namespace Core.Models
{
    public class GetObjectResponseDto<T> : ResponseDto
    {
        public T? Object { get; set; }
    }
}
