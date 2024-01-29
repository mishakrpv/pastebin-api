namespace WebApi.InputModels
{
    public class UploadTextModel
    {
        public string ContentBody { get; set; } = null!;
        public int? lifetimeInMinutes { get; set; }
    }
}
