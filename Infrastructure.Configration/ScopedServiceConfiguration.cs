
using Microsoft.Extensions.DependencyInjection;
using Service.Data;
using Service.Interface;

namespace Presentation.Config.ConfigurationService
{
    public static class ScopedServiceConfiguration
    {
        public static IServiceCollection AddScopedService(this IServiceCollection services)
        {

            services.AddTransient(typeof(IGatewayService), typeof(GatewayService));
            services.AddTransient(typeof(IPeripheralDeviceService), typeof(PeripheralDeviceService));
           
            return services;
        }
    }
}
