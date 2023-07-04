using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chateo;

public static class PreferencesExtensions
{
    public static T? GetFromJson<T>(this IPreferences preferences, string key, T? defaultValue = default)
    {
        var json = preferences.Get(key, (string?)null);

        if (json == null)
        {
            return defaultValue;
        }

        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }

    public static void SetAsJson<T>(this IPreferences preferences, string key, T value)
    {
        preferences.Set(key, System.Text.Json.JsonSerializer.Serialize(value));
    }
}