using Karin.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Karin.Cli
{
    public class Startup : BackgroundService
    {
        private Downloader _downloader;
        private DownloadOptions _downloadOptions;
        private IHostApplicationLifetime _lifetime;

        public Startup(
            Downloader downloader,
            IOptions<DownloadOptions> downloadOptions,
            IHostApplicationLifetime lifetime)
        {
            ArgumentNullException.ThrowIfNull(downloader, nameof(downloader));
            ArgumentNullException.ThrowIfNull(downloadOptions.Value, nameof(downloadOptions));
            ArgumentNullException.ThrowIfNull(lifetime, nameof(lifetime));

            _downloader = downloader;
            _downloadOptions = downloadOptions.Value;
            _lifetime = lifetime;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (string.IsNullOrWhiteSpace(_downloadOptions.InputUri))
            {
                throw new InvalidOperationException("No input Uri provided.");
            }

            if (string.IsNullOrWhiteSpace(_downloadOptions.OutputUri))
            {
                throw new InvalidOperationException("No output Uri provided.");
            }

            await _downloader.DownloadAsync(
                _downloadOptions.InputUri, 
                _downloadOptions.OutputUri);

            _lifetime.StopApplication();
        }
    }
}
