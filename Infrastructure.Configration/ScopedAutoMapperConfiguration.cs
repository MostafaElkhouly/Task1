
using AutoMapper;
using Infrastructure.ViewModel.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Configration.Configrations
{
    public static class ScopedAutoMapperConfiguration
    {
        public static IServiceCollection AddScopedAutoMapper(this IServiceCollection services)
        {

            var mappingConfig = new MapperConfiguration(mapper =>
            {
                mapper.AddProfile(new PeripheralDeviceProfile());
                mapper.AddProfile(new GatewayProfile());
            });

            //mappingConfig.AssertConfigurationIsValid();

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);


            return services;
        }
    }
}
