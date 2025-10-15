namespace Rental.Api.Application.DTOs.Courier
{
    public class GetDriverLicenseUrlResponse
    {
        public string? PresignedUrl { get; set; }

        public GetDriverLicenseUrlResponse() { }

        public GetDriverLicenseUrlResponse(string? presignedUrl)
        {
            PresignedUrl = presignedUrl;
        }
    }
}
