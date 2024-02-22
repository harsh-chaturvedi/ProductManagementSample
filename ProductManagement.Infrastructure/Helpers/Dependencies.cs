using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Infrastructure.Repository;
using ProductManagement.Infrastructure.Services;

namespace ProductManagment.Infrastructure.Helpers
{
    /// <summary>
    /// A class to register all API layer dependencies.
    /// </summary>
    public static class Dependencies
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProductRepository, ProductRepository>();
            return services;
        }

    }
}
