using Core;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebApi.InputModels;

namespace WebApi.Controllers
{
    public class TextController : ApiControllerBase
    {
        private readonly IObjectStorageService<TextObject> _storageService;

        public TextController(IObjectStorageService<TextObject> storageService)
        {
            _storageService = storageService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] UploadTextModel model)
        {
            var text = new TextObject(model.ContentBody);
            var response = await _storageService.UploadAsync(text, model.lifetimeInMinutes ?? Constants.DEFAULT_LIFETIME_IN_MINUTES);
            
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(new { Key = response.Key });
            }
            else
            {
                return StatusCode(response.StatusCode, response.Message);
            }
        }

        [HttpGet]
        [Route("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            var response = await _storageService.GetAsync(key);

            if (response.StatusCode == (int)HttpStatusCode.OK && response.Object != null)
            {
                return Content(response.Object.ContentBody);
            }
            else
            {
                return StatusCode(response.StatusCode, response.Message);
            }
        }
    }
}
