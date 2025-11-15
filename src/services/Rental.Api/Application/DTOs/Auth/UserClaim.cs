using Rental.Api.Swagger;

namespace Rental.Api.Application.DTOs.Auth
{
    public class UserClaim : IExposeInSwagger
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
