using Karin.Core;
using Microsoft.Extensions.Options;

namespace Karin.Infrastructure
{
    public class HttpPlaylistFetcher : IPlaylistFetcher
    {
        private readonly HttpClient _client;
        private readonly DownloadOptions _options;

        public HttpPlaylistFetcher(HttpClient client, IOptions<DownloadOptions> options)
        {
            ArgumentNullException.ThrowIfNull(client, nameof(client));
            ArgumentNullException.ThrowIfNull(options?.Value, nameof(options));

            _options = options.Value;

            _client = client;
            _client.DefaultRequestHeaders.Add(
                "User-Agent", _options.UserAgent);
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
