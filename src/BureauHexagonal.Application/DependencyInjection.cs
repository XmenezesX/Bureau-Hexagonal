using BureauHexagonal.Application.UseCases.CEP;
using BureauHexagonal.Application.UseCases.CEP.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace BureauHexagonal.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services
                    .AddUseCases();
        }

        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<IBureauCepUseCase, BureauCepUseCase>();
            services.AddScoped<IBureauCepUseCaseValidation, BureauCepUseCaseValidation>();


            return services;
        }

    }
}
