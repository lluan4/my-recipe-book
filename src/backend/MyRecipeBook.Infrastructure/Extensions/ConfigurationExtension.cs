using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MyRecipeBook.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {

        public static string ConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("Connection")!;
        }

        public static bool IsUnitTestEnviroment(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>("InMemoryTest");
        }

    }
}
