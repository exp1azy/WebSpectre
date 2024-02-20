using WebSpectre.Client.Data;
using WebSpectre.Server.Hubs;
using WebSpectre.Server.Hubs.Interfaces;
using WebSpectre.Server.Repositories;
using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Server.Services;
using WebSpectre.Server.Services.Interfaces;
using WebSpectre.Server.Startup;

namespace WebSpectre.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.AddElastic();
            builder.AddRedis();

            builder.Services.AddSignalR();

            builder.Services.AddDbContext<DataContext>();

            builder.Services.AddScoped<INetworkHub, NetworkHub>();
            builder.Services.AddTransient<IRedisRepository, RedisRepository>();
            builder.Services.AddTransient<IMonitoringService, MonitoringService>();
            builder.Services.AddTransient<PerfomanceCalculator>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<NetworkHub>("/networkhub");

            app.Run();
        }
    }
}
