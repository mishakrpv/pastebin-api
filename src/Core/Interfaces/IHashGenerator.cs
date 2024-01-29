namespace Core.Interfaces
{
    public interface IHashGenerator
    {
        Task<string> GetHashAsync();
    }
}
