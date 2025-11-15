using Rental.Api.Swagger;
using Rental.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Rental.Api.Application.DTOs.Auth
{
    public class UserLoginResponse : IResponse, IExposeInSwagger
    {
        public string AccessToken { get; set; }
        public Guid RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserToken UserToken { get; set; }
    }
}
