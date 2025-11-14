using Rental.Api.Application.DTOs.Auth;
using Rental.Api.Entities.Identity;

namespace Rental.Api.Application.Extensions
{
    public static class IdentityExtensions
    {
        public static UserRegisterResponse ToUserRegisterResponse(this ApplicationUser user)
        {
            if (user == null) return null;

            return new UserRegisterResponse
            {   Id = user.Id,
                Email = user.Email
            };
        }

    }
}
