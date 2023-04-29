namespace Karin.Core
{
    public interface IPlaylistFetcher
    {
        IAsyncEnumerable<string> FetchAsync(string uri);
    }
}