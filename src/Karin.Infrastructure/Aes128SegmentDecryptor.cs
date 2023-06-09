﻿using Karin.Core;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Karin.Infrastructure
{
    public class Aes128SegmentDecryptor : ISegmentFetcher
    {
        private HttpClient _client;
        private ISegmentFetcher _fetcher;
        private DownloadOptions _options;

        public Aes128SegmentDecryptor(
            HttpClient client, 
            ISegmentFetcher fetcher,
            IOptions<DownloadOptions> options)
        {
            ArgumentNullException.ThrowIfNull(client, nameof(client));
            ArgumentNullException.ThrowIfNull(fetcher, nameof(fetcher));
            ArgumentNullException.ThrowIfNull(options?.Value, nameof(options));

            _options = options.Value;

            _client = client;
            _client.DefaultRequestHeaders.Add(
                "User-Agent", _options.UserAgent);

            _fetcher = fetcher;
        }

        public async Task<byte[]> FetchAsync(Playlist playlist, MediaSegment segment)
        {
            var encrypted = await _fetcher.FetchAsync(playlist, segment);

            if (playlist.Encryption == null)
            {
                return encrypted;
            }

            if (playlist.Encryption.Method != "AES-128")
            {
                return encrypted;
            }

            var key = await GetKey(playlist.Encryption.Parameters["URI"]);
            var iv = await GetIV(segment.SequenceNumber);

            using var aes = Aes.Create();

            aes.KeySize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var decryptor = aes.CreateDecryptor(key, iv);

            using var ms = new MemoryStream(encrypted);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);

            var output = new byte[encrypted.Length];
            cs.Read(output, 0, encrypted.Length);

            return output;
        }

        private async Task<byte[]> GetKey(string keyUri)
        {
            var response = await _client.GetAsync(keyUri);
            var key = await response.Content.ReadAsByteArrayAsync();

            return key;
        }

        private async Task<byte[]> GetIV(int sequenceNumber)
        {
            var sequenceNumBytes =
                BitConverter.GetBytes(sequenceNumber)
                    .Reverse()
                    .ToArray();

            var iv = new byte[16];
            Array.Copy(sequenceNumBytes, 0, iv, iv.Length - sequenceNumBytes.Length, sequenceNumBytes.Length);

            await Task.CompletedTask;
            return iv;
        }
    }
}
