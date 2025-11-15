using Rental.Api.Application.DTOs.Auth;
using Rental.Core.Resources;
using Rental.Core.Responses;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

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
            data: new UserLoginResponse
            {
                AccessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxZWI3OTNhZS04ZmY3LTRkNDMtODRiYi02ODc4ZjM3Yjg3ZmUiLCJlbWFpbCI6InVzZXJAbWFpbC5jb20iLCJqdGkiOiI4N2JhZjMzOS1jNWVkLTQzNTgtYWZkNC0zZjhjYWFmNWRhZTMiLCJuYmYiOjE3MTE0MjAxMDAsImlhdCI6MTcxMTQyMDEwMCwiZXhwIjoxNzExNDI3MzAwLCJyb2xlIjoiAdmin\"}",
                RefreshToken = Guid.Parse("3f9d8a23-6a1c-47e8-b872-410f5e5d45d3"),
                ExpiresIn = 7200,
                UserToken = new UserToken
                {
                    Id = Guid.Parse("1eb793ae-8ff7-4d43-84bb-6878f37b887f"),
                    Email = "user@mail.com",
                    Claims = new List<UserClaim>
                    {
                        new UserClaim { Type = "email", Value = "john.doe@mail.com" },
                        new UserClaim { Type = "role",  Value = "User" },
                        new UserClaim { Type = "role",  Value = "Manager" },
                        new UserClaim { Type = "sub",   Value = "46c8e112-b154-4d39-8c65-7b32e8bbef20" }
                    }
                }
            }
        );


    }
}
