using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Infrastructure.Context;
using System.Reflection;

namespace ProductManagment.Helpers
{
    /// <summary>
    /// A class to register all API layer dependencies.
    /// </summary>
    public static class Dependencies
    {
        public static IServiceCollection RegisterApplicationDependencies(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
        {
            string _migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
            const int COMMAND_TIMEOUT = 240;

            services.AddHttpContextAccessor();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
               new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]),
               (opts) => { opts.MigrationsAssembly(_migrationsAssembly); opts.CommandTimeout(COMMAND_TIMEOUT); }),
               ServiceLifetime.Transient);
            return services;
        }

    }
}
