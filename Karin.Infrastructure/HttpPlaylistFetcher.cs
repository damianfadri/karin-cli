using Karin.Core;

namespace Karin.Infrastructure
{
    public class HttpPlaylistFetcher : IPlaylistFetcher
    {
        private readonly HttpClient _client;

        public HttpPlaylistFetcher(HttpClient client)
        {
            ArgumentNullException.ThrowIfNull(client, nameof(client));

            _client = client;
            _client.DefaultRequestHeaders.Add(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/112.0");
        }

        public async IAsyncEnumerable<string> FetchAsync(string uri)
        {
            var response = await _client.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();

            var lines = content.Split('\n')
                .Select(line => line.Trim())
                .Where(line => line.Length > 0)
                .Where(line => line.StartsWith("#EXT") || !line.StartsWith("#"));

            foreach (var line in lines)
            {
                yield return line;
            }
        }
    }
}
