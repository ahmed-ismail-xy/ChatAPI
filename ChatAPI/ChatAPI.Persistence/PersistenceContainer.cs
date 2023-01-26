using ChatAPI.Application.Contracts;
using ChatAPI.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatAPI.Persistence
{
    public static class PersistenceContainer
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped(typeof(IAuthRepository), typeof(AuthRepository));
            services.AddScoped(typeof(IChatRepository), typeof(ChatRepository));
            services.AddScoped(typeof(IMessageReposiory), typeof(MessageRepository));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

            return services;
        }
    }
}
