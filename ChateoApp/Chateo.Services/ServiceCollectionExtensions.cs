using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Services;

public static class ServiceCollectionExtensions
{
    public static void AddChatServices(this IServiceCollection services, Uri serverUri)
    {
        services.AddSingleton<IChatServer, Implementation.ChatServer>();

        services.AddHttpClient("ChatServer", httpClient =>
        {
            httpClient.BaseAddress = serverUri;
        });
    }
}
