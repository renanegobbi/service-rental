namespace Rental.Core.Authorization
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string User = "User";

        public const string AdminOrManager = Admin + "," + Manager;
        public const string AllAuthenticatedUsers = Admin + "," + Manager + "," + User;
    }
}
