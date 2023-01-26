using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatAPI.Persistence
{
    public static class PersistenceContainer
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ChatServiceDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IAuthRepository), typeof(AuthRepository));
            services.AddScoped(typeof(IChatRepository), typeof(ChatRepository));
            services.AddScoped(typeof(IMessageReposiory), typeof(MessageRepository));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

            return services;
        }
    }
}
