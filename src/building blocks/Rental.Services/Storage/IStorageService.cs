using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Rental.Api.Services.Storage
{
    public interface IStorageService
    {
        Task<string> UploadAsync(Stream stream, string objectName, string contentType, CancellationToken ct = default);
        Task<bool> ExistsAsync(string objectName, CancellationToken ct = default);
        Task DeleteAsync(string objectName, CancellationToken ct = default);
        Task<string> GetPresignedUrlAsync(string objectName, TimeSpan expiresIn, CancellationToken ct = default);
        Task EnsureBucketAsync(CancellationToken ct = default);
    }
}
