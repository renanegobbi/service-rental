using Rental.Api.Swagger;
using Rental.Core.Interfaces;
using System;

namespace Rental.Api.Application.DTOs.Auth
{
    public class UserRegisterResponse : IResponse, IExposeInSwagger
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
