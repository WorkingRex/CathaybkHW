using CathaybkHW.Application;
using CathaybkHW.Domain.DomainServices.Currency.Interface;
using CathaybkHW.Infrastructure.ExternalServices.CoindeskAPI;
using CathaybkHW.Infrastructure.Repositories;
using System.Reflection;

namespace CathaybkHW.Extensions;

public static class ServicesExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetAssembly(typeof(ApplicationRegistration))!);
        });
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
    }
}
