using Microsoft.AspNetCore.SignalR.Client;
using WebSpectre.Client.Services;

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

            services.AddSingleton(sp =>
            {
                var httpClient = sp.GetService<HttpClient>();
                var hubConnection = sp.GetService<HubConnection>();

                return new NetworkHandler(httpClient, hubConnection);
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
