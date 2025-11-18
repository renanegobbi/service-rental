namespace Rental.Core.Authorization
{
    public static class AuthorizationPolicies
    {
        public const string AdminOnly = "AdminOnly";
        public const string ManagerOnly = "ManagerOnly";
        public const string UserOnly = "UserOnly";
        public const string AdminOrManager = "AdminOrManager";

        public const string AllAuthenticatedUsers = "AllAuthenticatedUsers";
    }
}
