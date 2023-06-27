namespace Karin.Core
{
    public class Downloader
    {
        private IPlaylistFetcher _playlistFetcher;
        private ISegmentFetcher _segmentFetcher;
        private IEnumerable<IOutputStrategy> _outputs;

        public Downloader(
            IPlaylistFetcher playlistFetcher,
            ISegmentFetcher segmentFetcher,
            IEnumerable<IOutputStrategy> outputs)
        {
            ArgumentNullException.ThrowIfNull(playlistFetcher, nameof(playlistFetcher));
            ArgumentNullException.ThrowIfNull(segmentFetcher, nameof(segmentFetcher));
            ArgumentNullException.ThrowIfNull(outputs, nameof(outputs));

            _playlistFetcher = playlistFetcher;
            _segmentFetcher = segmentFetcher;
            _outputs = outputs;
        }

        public async Task DownloadAsync(string source, string destination)
        {
            var playlist = await ParseAsync(source);
            foreach (var segment in playlist.Segments)
            {
                var segmentBytes = await _segmentFetcher.FetchAsync(playlist, segment);

                foreach (var output in _outputs)
                {
                    await output.OutputAsync(segmentBytes, destination);
                }
            }
        }

        private async Task<Playlist> ParseAsync(string playlistUri)
        {
            var builder = new PlaylistBuilder();
            var lines = _playlistFetcher.FetchAsync(playlistUri).GetAsyncEnumerator();

            while (await lines.MoveNextAsync())
            {
                var currentLine = lines.Current;
                if (!currentLine.StartsWith("#EXT"))
                {
                    continue;
                }

                var pair = new KeyValuePair(currentLine);

                switch (pair.Key)
                {
                    case "#EXT-X-KEY":
                        var encryption = new MultiValueString(pair.Value);

                        builder.SetEncryption(
                            new Encryption(
                                encryption.Values["METHOD"],
                                encryption.Values));

                        break;

                    case "#EXT-X-MEDIA-SEQUENCE":
                        builder.SetSequenceNumberStart(
                            Convert.ToInt32(pair.Value));

                        break;

                    case "#EXTINF":
                        await lines.MoveNextAsync();
                        builder.AddSegment(lines.Current);

                        break;
                }
            }

            return builder.Build();
        }
    }
}
