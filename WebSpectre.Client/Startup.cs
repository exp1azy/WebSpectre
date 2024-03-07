using Microsoft.AspNetCore.SignalR.Client;

namespace WebSpectre.Client
{
    public static class Startup
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient(sp =>
            {
                var config = sp.GetService<IConfiguration>();

                return new HubConnectionBuilder()
                    .WithUrl($"{config.GetApiUrl()}/networkHub")
                    .Build();
            });

            services.AddScoped(sp =>
            {
                var config = sp.GetService<IConfiguration>();

                return new HttpClient { BaseAddress = new Uri($"{config.GetApiUrl()}/") };
            });
        }

        public static string GetApiUrl(this IConfiguration config) 
        {
            var apiUrl = config["ApiUrl"];

            if (apiUrl == null)
                return string.Empty;

            return apiUrl;
        }
    }
}
