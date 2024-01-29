﻿using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services
{
    public class S3TextStorageService : IObjectStorageService<TextObject>
    {
        private string _backetName = "pastebinapi";

        private readonly AWSCredentials _credentials;
        private readonly AmazonS3Config _configsS3 = new AmazonS3Config()
        {
            ServiceURL = "https://s3.yandexcloud.net"
        };
        private readonly IAppLogger<S3TextStorageService> _logger;
        private readonly IRepository<ObjectDetails> _objectDetailsRepository;
        private readonly IHashGenerator _hashGenerator;

        public S3TextStorageService(
            IOptions<AwsCredentials> options,
            IAppLogger<S3TextStorageService> logger,
            IRepository<ObjectDetails> objectDetailsRepository,
            IHashGenerator hashGenerator)
        {
            var credentials = options.Value;
            _credentials = new BasicAWSCredentials(credentials.AwsKey, credentials.AwsSecretKey);
            _logger = logger;
            _objectDetailsRepository = objectDetailsRepository;
            _hashGenerator = hashGenerator;
        }

        public async Task<UploadObjectResponseDto> UploadAsync(TextObject objectItem, int lifetimeInMinutes)
        {
            string key = await _hashGenerator.GetHashAsync();
            var response = await UploadToS3Async(objectItem.ContentBody, key);

            if (response.StatusCode == 200)
            {
                _logger.LogInformation(response.Message);
                response.Key = key;
                await _objectDetailsRepository.AddAsync(new ObjectDetails(key, DateTime.UtcNow.AddMinutes(lifetimeInMinutes)));
            }
            else
            {
                _logger.LogError(response.Message);
            }

            return response;
        }

        public async Task<UploadObjectResponseDto> UploadToS3Async(string contentBody, string key)
        {
            using var s3Client = new AmazonS3Client(_credentials, _configsS3);

            var response = new UploadObjectResponseDto();

            var putRequest = new PutObjectRequest()
            {
                ContentBody = contentBody,
                Key = key,
                BucketName = _backetName,
                CannedACL = S3CannedACL.NoACL,
            };

            try
            {
                await s3Client.PutObjectAsync(putRequest);

                response.StatusCode = 200;
                response.Message = $"{key} object has been uploaded successfully";
            }
            catch (AmazonS3Exception ex)
            {
                response.StatusCode = (int)ex.StatusCode;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<GetObjectResponseDto<TextObject>> GetAsync(string key)
        {
            var objectDetailsSpec = new ObjectDetailsSpecification(key);
            var objectDetails = await _objectDetailsRepository.FirstOrDefaultAsync(objectDetailsSpec);

            if (objectDetails != null)
            {
                if (objectDetails.Expiration > DateTime.UtcNow)
                {
                    var response = await GetFromS3Async(key);

                    if (response.StatusCode == 200)
                    {
                        _logger.LogInformation(response.Message);
                    }
                    else
                    {
                        _logger.LogError(response.Message);
                    }

                    return response;
                }
                else
                {
                    return new GetObjectResponseDto<TextObject>()
                        {
                            StatusCode = 200,
                            Message = "Link has expired"
                        };
                }
            }
            else
            {
                return new GetObjectResponseDto<TextObject>()
                    {
                        StatusCode = 404,
                        Message = "Key not found"
                    };
            }
        }

        public async Task<GetObjectResponseDto<TextObject>> GetFromS3Async(string key)
        {
            using var s3Client = new AmazonS3Client(_credentials, _configsS3);

            var responseDto = new GetObjectResponseDto<TextObject>();

            var getRequest = new GetObjectRequest()
            {
                Key = key,
                BucketName = _backetName,
            };

            try
            {
                using GetObjectResponse response = await s3Client.GetObjectAsync(getRequest);

                using (StreamReader reader = new StreamReader(response.ResponseStream))
                {
                    responseDto.Object = new TextObject(reader.ReadToEnd());
                }

                responseDto.StatusCode = 200;
                responseDto.Message = $"{key} object read successfully";
            }
            catch (AmazonS3Exception ex)
            {
                responseDto.StatusCode = (int)ex.StatusCode;
                responseDto.Message = ex.Message;
            }
            catch (Exception ex)
            {
                responseDto.StatusCode = 500;
                responseDto.Message = ex.Message;
            }

            return responseDto;
        }
    }
}
