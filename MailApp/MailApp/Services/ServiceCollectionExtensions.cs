using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailApp.Services;

static class ServiceCollectionExtensions
{
    public static void AddMailAppMauiServices(this IServiceCollection services)
    {
        services.AddSingleton<ISecureStorage, SecureStorage>();
    }
}
