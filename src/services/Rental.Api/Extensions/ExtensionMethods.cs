using Microsoft.Extensions.Hosting;

namespace Rental.Api.Extensions
{
    namespace Rental.Api.Extensions
    {
        public static partial class ExtensionMethods
        {
            public static string GetDescription(this IHostEnvironment environment)
            {
                switch (environment.EnvironmentName)
                {
                    case "Development":
                        return "Development";
                    case "Staging":
                        return "Staging";
                    case "Production":
                        return "Production";
                    default: return string.Empty;
                }
            }

            public static string GetEnvironmentAbbreviation(this IHostEnvironment environment)
            {
                switch (environment.EnvironmentName)
                {
                    case "Development":
                        return "DEV";
                    case "Staging":
                        return "STG";
                    case "Production":
                        return "PRD";
                    default: return string.Empty;
                }
            }
        }
    }
}
