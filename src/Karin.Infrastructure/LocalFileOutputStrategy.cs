using Karin.Core;

namespace Karin.Infrastructure
{
    public class LocalFileOutputStrategy : IOutputStrategy
    {
        public async Task OutputAsync(byte[] data, string destination)
        {
            using var fs = new FileStream(destination, FileMode.Append);
            using var bw = new BinaryWriter(fs);

            bw.Write(data);

            await Task.CompletedTask;
        }
    }
}
