namespace Rental.Api.Application.DTOs.Courier
{
    public class UploadDriverLicenseResponse
    {
        public string ObjectKey { get; set; }
        public string? PresignedUrl { get; set; }

        public UploadDriverLicenseResponse() { }

        public UploadDriverLicenseResponse(string objectKey, string? presignedUrl)
        {
            ObjectKey = objectKey;
            PresignedUrl = presignedUrl;
        }
    }
}
