using Karin.Core;

namespace Karin.Infrastructure
{
    public class HttpSegmentFetcher : ISegmentFetcher
    {
        private readonly HttpClient _client;

        public HttpSegmentFetcher(HttpClient client)
        {
            ArgumentNullException.ThrowIfNull(client, nameof(client));

            _client = client;
        }

        public async Task<byte[]> FetchAsync(Playlist playlist, MediaSegment segment)
        {
            var response = await _client.GetAsync(segment.Uri);
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
