using Persistence.IRepository;
using Persistence.IRepository.IEntityRepository;
using Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Config.ConfigurationService
{
    public static class ScopedRepositoryConfiguration
    {
        public static IServiceCollection AddScopedRepository(this IServiceCollection services)
        {
            services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            //services.AddTransient(typeof(IUserRepository), typeof(UserRepository));
            //services.AddTransient(typeof(IRoleRepository), typeof(RoleRepository));

            return services;
        }
    }
}
