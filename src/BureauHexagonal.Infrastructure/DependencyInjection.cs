using BureauHexagonal.Application.UnitOfWork;
using BureauHexagonal.Core.Enums;
using BureauHexagonal.Core.Ports;
using BureauHexagonal.Infrastructure.Adapters.Cep;
using BureauHexagonal.Infrastructure.DataBase.Postgres;
using BureauHexagonal.Infrastructure.DataBase.Postgres.UnitOfWork;
using BureauHexagonal.Infrastructure.Gateways.BrasilApi;
using BureauHexagonal.Infrastructure.Gateways.ViaCep;
using BureauHexagonal.Infrastructure.Options.DataBase;
using BureauHexagonal.Infrastructure.Options.Provider;
using BureauHexagonal.Infrastructure.Repository;
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
                   .AddPortsAndAdaptersServices()
                   .AddRepository(configuration)
                   .AddViaCep(configuration)
                   .AddBrasilApi(configuration);
        }

        private static IServiceCollection AddPortsAndAdaptersServices(this IServiceCollection services)
        {
            services.AddKeyedScoped<ISearchCepPort, SearchViaCepAdapter>(ProviderType.ViaCep);
            services.AddKeyedScoped<ISearchCepPort, SearchBrasiApiCepAdapter>(ProviderType.BrasilApi);

            return services;
        }

        private static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var dataBaseOptions = configuration.GetRequiredSection(DataBaseOptions.SectionName).Get<DataBaseOptions>()!;

            if (dataBaseOptions.UseDataBaseType == StorageType.Postgres)
                return services.AddPostgress(configuration);

            if (dataBaseOptions.UseDataBaseType == StorageType.Dynamo)
            {
                
            }

            return services;
        }

        private static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<DataBaseOptions>().Bind(configuration.GetSection(DataBaseOptions.SectionName));
            services.AddOptions<PostgresDbOptions>().Bind(configuration.GetSection(PostgresDbOptions.SectionName));
            services.AddOptions<ViaCepOptions>().Bind(configuration.GetSection(ViaCepOptions.SectionName));
            services.AddOptions<BrasilApiOptions>().Bind(configuration.GetSection(BrasilApiOptions.SectionName));

            return services;
        }

        private static IServiceCollection AddPostgress(this IServiceCollection services, IConfiguration configuration)
        {
            var postgresOptions = configuration.GetRequiredSection(PostgresDbOptions.SectionName).Get<PostgresDbOptions>()!;
            services.AddDbContext<BureauPostgresDbContext>(options =>
                options.UseNpgsql(postgresOptions.GetConnectionString()));

            services.AddScoped<IBureauRepositoryPort, BureauPostgresRepositoryAdapter>();
            services.AddScoped<IUnitOfWork, PostgresUnitOfWork>();

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

        private static IServiceCollection AddBrasilApi(this IServiceCollection services, IConfiguration configuration)
        {
            var brasilApiOptions = configuration.GetRequiredSection(BrasilApiOptions.SectionName).Get<BrasilApiOptions>()!;

            services.AddRefitClient<IBrasilApiGateway>(new RefitSettings()
            {
                ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                })
            })
            .ConfigureHttpClient(httpClient =>
            {
                httpClient.BaseAddress = new Uri(brasilApiOptions.BaseUrl);
                httpClient.Timeout = new TimeSpan(0, 0, brasilApiOptions.TimeoutInSeconds);
                httpClient.DefaultRequestHeaders.Clear();
            });

            return services;
        }
    }
}
