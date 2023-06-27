namespace Karin.Core
{
    public class PlaylistBuilder
    {
        private int _sequenceNumberStart;
        private Encryption? _encryption;
        private List<MediaSegment> _segments;

        public PlaylistBuilder()
        {
            _segments = new List<MediaSegment>();
        }

        public PlaylistBuilder SetEncryption(Encryption encryption)
        {
            _encryption = encryption;
            return this;
        }

        public PlaylistBuilder SetSequenceNumberStart(int sequenceNumberStart)
        {
            _sequenceNumberStart = sequenceNumberStart;
            return this;
        }

        public PlaylistBuilder AddSegment(string uri)
        {
            var sequencedSegment
                = new MediaSegment(
                    uri,
                    _sequenceNumberStart + _segments.Count);

            _segments.Add(sequencedSegment);
            return this;
        }

        public Playlist Build()
        {
            return new Playlist(
                _segments,
                _sequenceNumberStart,
                _encryption);
        }
    }
}
