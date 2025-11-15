using System.Collections.Generic;
using System;
using Rental.Api.Swagger;

namespace Rental.Api.Application.DTOs.Auth
{
    public class UserToken : IExposeInSwagger
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaim> Claims { get; set; }
    }
}
