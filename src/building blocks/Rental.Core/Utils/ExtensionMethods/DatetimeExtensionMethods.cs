namespace Rental.Core.Utils.ExtensionMethods
{
    public static partial class ExtensionMethods
    {
        public static bool BeAtLeast18YearsOld(this DateTime input)
        {
            var age = DateTime.Today.Year - input.Year;
            if (input.Date > DateTime.Today.AddYears(-age)) age--;
            return age >= 18;
        }
    }
}
