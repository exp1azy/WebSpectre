using Elasticsearch.Net;
using Nest;
using Error = WebSpectre.Server.Resources.Error;

namespace WebSpectre.Server.Startup
{
    public static class ElasticConfig
    {
        public static void AddElastic(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(sp =>
            {
                var connection = builder.Configuration.GetConnectionString("ElasticConnection");
                if (string.IsNullOrEmpty(connection))
                    throw new ArgumentNullException(Error.FailedToReadElasticConnectionString);

                var authParams = builder.Configuration.GetSection("ElasticSearchAuth");
                if (string.IsNullOrEmpty(authParams["Username"]) || string.IsNullOrEmpty(authParams["Password"]))
                    throw new ArgumentNullException(Error.FailedToReadESAuthParams);

                var settings = new ConnectionSettings(new Uri(connection))
                    .BasicAuthentication(authParams["Username"], authParams["Password"])
                    .ServerCertificateValidationCallback((o, certificate, chain, errors) => true)
                    .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
                    .DisableDirectStreaming();

                return new ElasticClient(settings);
            });
        }
    }
}
