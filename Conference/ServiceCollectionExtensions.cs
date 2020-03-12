using System;
using Microsoft.Extensions.DependencyInjection;

namespace Conference
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControllers(this IServiceCollection services, Action<IMvcBuilder> configure)
        {
            configure(services.AddControllers());
            return services;
        }
    }
}
