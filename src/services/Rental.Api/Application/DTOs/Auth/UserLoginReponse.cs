using Rental.Api.Swagger;
using Rental.Core.Interfaces;

namespace Rental.Api.Application.DTOs.Auth
{
    public class UserLoginResponse : IResponse, IExposeInSwagger
    {
        public string Email { get; set; }
    }
}
