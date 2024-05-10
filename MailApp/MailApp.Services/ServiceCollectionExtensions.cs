using MailApp.Services.Data;
using Microsoft.Extensions.DependencyInjection;
using ReactorData.EFCore;
using Shiny;
using Shiny.Jobs;

namespace MailApp.Services;

public static class ServiceCollectionExtensions
{
    public static void AddMailAppServices(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>();

        services.AddReactorData<ApplicationDbContext>();
    }
}
