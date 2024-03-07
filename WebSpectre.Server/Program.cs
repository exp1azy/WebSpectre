using Microsoft.AspNetCore.ResponseCompression;
using Serilog;
using WebSpectre.Client.Data;
using WebSpectre.Server.Hubs;
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

            builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
            {
                p.AllowAnyHeader();
                p.AllowAnyMethod();
                p.AllowAnyOrigin();
            }));

            builder.Services.AddSignalR();
            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                      ["application/octet-stream"]);
            });

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(builder.Configuration["LogFile"]!)
                .CreateLogger();

            builder.Services.AddDbContext<DataContext>();

            builder.Services.AddTransient<IRedisRepository, RedisRepository>();
            builder.Services.AddTransient<IMonitoringService, MonitoringService>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<PerfomanceCalculator>();

            var app = builder.Build();

            app.UseResponseCompression();
            
            app.UseCors();

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
