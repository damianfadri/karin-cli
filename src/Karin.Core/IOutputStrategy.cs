namespace Karin.Core
{
    public interface IOutputStrategy
    {
        Task OutputAsync(byte[] data, string destination);
    }
}
