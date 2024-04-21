using CathaybkHW.Infrastructure.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CathaybkHW.Infrastructure.Extensions;

public static class HttpClientExtensions
{
    public static IServiceCollection RegisterHttpClient(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddHttpClient(nameof(HttpClientNames.CoindeskAPI), client =>
        {
            client.BaseAddress = new Uri(configuration.GetSection("CoindeskAPI").GetValue<string>("BaseUrl")!);
        });

        return service;
    }
}
