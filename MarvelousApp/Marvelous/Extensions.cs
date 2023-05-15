using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marvelous;

static class Extensions
{
    [return: NotNull]
    public static T ThrowIfNull<T>(this T? value) where T : class
        => value ?? throw new ArgumentNullException(nameof(value));
}
