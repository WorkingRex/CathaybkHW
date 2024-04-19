using CathaybkHW.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CathaybkHW.Infrastructure.Extensions;

public static class EFCoreExtensions
{
    public static IServiceCollection AddDBContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CathaybkHWDB");
        services.AddDbContext<CathaybkHWDBContext>(options => options.UseSqlServer(connectionString!));

        return services;
    }
}
