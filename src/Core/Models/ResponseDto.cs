namespace Core.Models
{
    public abstract class ResponseDto
    {
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = string.Empty;
    }
}
