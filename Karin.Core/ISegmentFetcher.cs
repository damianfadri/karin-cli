namespace Karin.Core
{
    public interface ISegmentFetcher
    {
        Task<byte[]> FetchAsync(Playlist playlist, MediaSegment segment);
    }
}