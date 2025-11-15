using Rental.Api.Application.DTOs.Auth;
using Rental.Core.Resources;
using Rental.Core.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Rental.Api.Swagger.Examples
{
    public class UserRegisterRequestExamplo : IExamplesProvider<UserRegisterRequest>
    {
        public UserRegisterRequest GetExamples() => new UserRegisterRequest
        {
            Email = "user@email.com",
            Password = "Senha@123",
            PasswordConfirmation = "Senha@123"
        };
    }

    public class UserRegisterResponseExamplo : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples() => new ApiResponse(
            success: true,
            messages: new[] { AuthMessages.User_Created_Successfully },
            data: new UserRegisterResponse()
            {
                Id = Guid.Parse("a39b592b-2116-4843-8959-d4919c092a9e"),
                Email = "user@email.com"
            }
        );
    }

    public class UserLoginRequestExamplo : IExamplesProvider<UserLoginRequest>
    {
        public UserLoginRequest GetExamples() => new UserLoginRequest
        {
            Email = "user@email.com",
            Password = "Senha@123"
        };
    }

    public class UserLoginResponseExamplo : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples() => new ApiResponse(
            success: true,
            messages: new[] { AuthMessages.User_Created_Successfully },
            data: new UserLoginResponse()
            {
                Email = "user@email.com"
            }
        );
    }
}
