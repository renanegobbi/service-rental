using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Rental.Services.User
{
    public interface IUser
    {
        string Name { get; }
        Guid GetUserId();
        string GetUserEmail();
        string GetUserBearerToken();
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetClaimsIdentity();
        string GetCorrelationId();
    }
}
