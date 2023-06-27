using Karin.Cli;
using Karin.Core;
using Karin.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    private static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddCommandLine(args, new Dictionary<string, string>
                {
                    { "--i", $"{nameof(DownloadOptions)}:{nameof(DownloadOptions.InputUri)}" },
                    { "--o", $"{nameof(DownloadOptions)}:{nameof(DownloadOptions.OutputUri)}" },
                    { "--agent", $"{nameof(DownloadOptions)}:{nameof(DownloadOptions.UserAgent)}" }
                });
            })
            .ConfigureServices((context, services) =>
            {
                var configuration = context.Configuration;

                services.AddTransient<Downloader>();

                services
                    .AddTransient<IPlaylistFetcher, HttpPlaylistFetcher>()
                    .AddHttpClient<IPlaylistFetcher, HttpPlaylistFetcher>();

                services
                    .AddTransient<ISegmentFetcher, HttpSegmentFetcher>()
                    .AddHttpClient<ISegmentFetcher, HttpSegmentFetcher>();

                services
                    .AddTransient<IOutputStrategy, LocalFileOutputStrategy>();

                services
                    .Decorate<ISegmentFetcher, Aes128SegmentDecryptor>();

                services.Configure<DownloadOptions>(
                    configuration.GetSection(nameof(DownloadOptions)));

                services.AddHostedService<Startup>();
            })
            .RunConsoleAsync();
    }
}