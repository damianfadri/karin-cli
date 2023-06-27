namespace Karin.Core
{
    public class Playlist
    {
        public int SequenceNumberStart { get; }
        public List<MediaSegment> Segments { get; }
        public Encryption? Encryption { get; }

        public Playlist(List<MediaSegment> segments)
            : this(segments, 0)
        {
        }

        public Playlist(List<MediaSegment> segments, int sequenceNumberStart)
            : this(segments, sequenceNumberStart, null)
        {
        }

        public Playlist(List<MediaSegment> segments, int sequenceNumberStart, Encryption? encryption)
        {
            Segments = segments;
            SequenceNumberStart = sequenceNumberStart;
            Encryption = encryption;
        }
    }
}