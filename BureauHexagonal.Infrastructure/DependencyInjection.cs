using BureauHexagonal.Core.Ports;
using BureauHexagonal.Infrastructure.Adapters.Cep;
using BureauHexagonal.Infrastructure.Adapters.Cep.Gateway;
using BureauHexagonal.Infrastructure.DataBase.Postgres;
using BureauHexagonal.Infrastructure.Options.DataBase;
using BureauHexagonal.Infrastructure.Options.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BureauHexagonal.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                   .AddOptions(configuration)
                   .AddPostgress(configuration)
                   .AddPortsAndAdapters()
                   .AddViaCep(configuration);
        }

        public static IServiceCollection AddPortsAndAdapters(this IServiceCollection services)
        {
            services.AddScoped<ISearchCepPort, SearchViaCepAdapter>();
            
            return services;
        }

        private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<PostgresDbOptions>().Bind(configuration.GetSection(PostgresDbOptions.SectionName));
            services.AddOptions<ViaCepOptions>().Bind(configuration.GetSection(ViaCepOptions.SectionName));

            return services;
        }

        private static IServiceCollection AddPostgress(this IServiceCollection services, IConfiguration configuration)
        {
            var postgresOptions = configuration.GetRequiredSection(PostgresDbOptions.SectionName).Get<PostgresDbOptions>()!;
            services.AddDbContext<BureauPostgresDbContext>(options =>
                options.UseNpgsql(postgresOptions.GetConnectionString()));

            return services;
        }

        private static IServiceCollection AddViaCep(this IServiceCollection services, IConfiguration configuration)
        {
            var viaCepOptios = configuration.GetRequiredSection(ViaCepOptions.SectionName).Get<ViaCepOptions>()!;

            services.AddRefitClient<IViaCepGateway>(new RefitSettings()
            {
                ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                })
            })
            .ConfigureHttpClient(httpClient =>
            {
                  httpClient.BaseAddress = new Uri(viaCepOptios.BaseUrl);
                  httpClient.Timeout = new TimeSpan(0, 0, viaCepOptios.TimeoutInSeconds);
                  httpClient.DefaultRequestHeaders.Clear();
            });

            return services;
        }
    }
}
