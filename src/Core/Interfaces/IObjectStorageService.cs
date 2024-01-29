using Core.Models;

namespace Core.Interfaces
{
    public interface IObjectStorageService<T> where T : class, IObjectItem
    {
        Task<GetObjectResponseDto<T>> GetAsync(string key);

        Task<UploadObjectResponseDto> UploadAsync(T objectItem, int lifetimeInMinutes);
    }
}
