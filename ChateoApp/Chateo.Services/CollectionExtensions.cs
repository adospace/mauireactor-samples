using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chateo.Services;

public static class CollectionExtensions
{
    public static IList<T> Replace<T>(this IList<T> values, Func<T, bool> findFunc, T newValue)
    {
        for (int i = 0; i < values.Count; i++)
        {
            if (findFunc(values[i]))
            {
                values[i] = newValue;
                break;
            }
        }
        return values;
    }

    public static T[] Replace<T>(this T[] values, Func<T, bool> findFunc, T newValue)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (findFunc(values[i]))
            {
                values[i] = newValue;
                break;
            }
        }

        return values;
    }
}
