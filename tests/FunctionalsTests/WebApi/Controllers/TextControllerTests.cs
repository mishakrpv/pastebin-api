using System.Net.Http.Json;

namespace FunctionalsTests.WebApi.Controllers
{
    public class TextControllerTests : IClassFixture<TestApplication>
    {
        public HttpClient Client { get; }

        public TextControllerTests(TestApplication factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task UploadsAlreadyExpiredText_ReturnsExpiredResult()
        {
            var uploadContent = new { ContentBody = "test", LifetimeInMinutes = 0 };
            var uploadResponse = await Client.PostAsync("text/upload", JsonContent.Create(uploadContent));
            uploadResponse.EnsureSuccessStatusCode();
            var uploadResult = await uploadResponse.Content.ReadFromJsonAsync<UploadResult>();
            Assert.NotNull(uploadResult);
            Assert.NotNull(uploadResult.Key);
            string key = uploadResult.Key;
        
            var getResponse = await Client.GetAsync($"text/get/{key}");
            getResponse.EnsureSuccessStatusCode();
            var stringResponse = await getResponse.Content.ReadAsStringAsync();

            Assert.Contains("Key has expired", stringResponse);
        }

        [Fact]
        public async Task UploadsText_ReturnsSuccessfulResultWithGivenText()
        {
            var uploadContent = new { ContentBody = "test\nUncle Bob is a cool guy\nNewLine", LifetimeInMinutes = 5 };
            var uploadResponse = await Client.PostAsync("text/upload", JsonContent.Create(uploadContent));
            uploadResponse.EnsureSuccessStatusCode();
            var uploadResult = await uploadResponse.Content.ReadFromJsonAsync<UploadResult>();
            Assert.NotNull(uploadResult);
            Assert.NotNull(uploadResult.Key);
            string key = uploadResult.Key;

            var getResponse = await Client.GetAsync($"text/get/{key}");
            getResponse.EnsureSuccessStatusCode();
            string stringResponse = await getResponse.Content.ReadAsStringAsync();
            
            Assert.Contains(uploadContent.ContentBody, stringResponse);
        }

        internal class UploadResult
        {
            public string? Key { get; set; }
        }
    }
}
