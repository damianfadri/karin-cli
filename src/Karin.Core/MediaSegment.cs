namespace Karin.Core
{
    public class MediaSegment
    {
        public string Uri { get; }
        public int SequenceNumber { get; }

        public MediaSegment(string uri)
        {
            Uri = uri;
        }

        public MediaSegment(string uri, int sequenceNumber)
        {
            Uri = uri;
            SequenceNumber = sequenceNumber;
        }
    }
}
