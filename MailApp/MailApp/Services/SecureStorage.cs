using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailApp.Services;

class SecureStorage : ISecureStorage
{
    public async Task<string?> Get(string key)
    {
        return await Microsoft.Maui.Storage.SecureStorage.Default.GetAsync(key);
    }

    public async Task Set(string key, string value)
    {
        await Microsoft.Maui.Storage.SecureStorage.Default.SetAsync(key, value);
    }
}
