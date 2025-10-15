using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Rental.Services.Storage;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Services.Storage
{
    public class MinioStorageService : IStorageService
    {
        private readonly IMinioClient _client;
        private readonly StorageOptions _opt;

        public MinioStorageService(IMinioClient client, IOptions<StorageOptions> opt)
        {
            _client = client;
            _opt = opt.Value;
        }

        public async Task EnsureBucketAsync(CancellationToken ct = default)
        {
            var exists = await _client.BucketExistsAsync(new BucketExistsArgs().WithBucket(_opt.Bucket), ct);
            if (!exists)
                await _client.MakeBucketAsync(new MakeBucketArgs().WithBucket(_opt.Bucket), ct);
        }

        public async Task<string> UploadAsync(Stream stream, string objectName, string contentType, CancellationToken ct = default)
        {
            await EnsureBucketAsync(ct);

            // PutObject
            var putArgs = new PutObjectArgs()
                .WithBucket(_opt.Bucket)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType);

            await _client.PutObjectAsync(putArgs, ct);
            return objectName;
        }

        public async Task<bool> ExistsAsync(string objectName, CancellationToken ct = default)
        {
            try
            {
                var stat = new StatObjectArgs().WithBucket(_opt.Bucket).WithObject(objectName);
                await _client.StatObjectAsync(stat, ct);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task DeleteAsync(string objectName, CancellationToken ct = default)
        {
            var del = new RemoveObjectArgs().WithBucket(_opt.Bucket).WithObject(objectName);
            await _client.RemoveObjectAsync(del, ct);
        }

        public async Task<string> GetPresignedUrlAsync(string objectName, TimeSpan expiresIn, CancellationToken ct = default)
        {
            var req = new PresignedGetObjectArgs()
                .WithBucket(_opt.Bucket)
                .WithObject(objectName)
                .WithExpiry((int)expiresIn.TotalSeconds);
            return await _client.PresignedGetObjectAsync(req);
        }
    }
}
