using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Server.Repositories;
using WebSpectre.Server.Services.Interfaces;
using WebSpectre.Server.Services;

namespace WebSpectre.Server.Startup
{
    public static class ServicesConfig
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IRedisRepository, RedisRepository>();
            builder.Services.AddTransient<IMonitoringService, MonitoringService>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAgentRepository, AgentRepository>();
            builder.Services.AddTransient<IAgentService, AgentService>();
            builder.Services.AddTransient<PerfomanceCalculator>();
        }
    }
}
