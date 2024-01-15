using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackizerApp.Models;

static class Utils
{
    public static string? GetDisplayName(this Enum enumValue)
    {
        // Get the type of the enum
        Type type = enumValue.GetType();

        // Get the member information for the specified enum value
        var memberInfo = type.GetMember(enumValue.ToString());

        // If there's at least one member with this name (there should be unless it's a non-defined value)
        if (memberInfo.Length > 0)
        {
            // Get the Display attribute if it exists
            var attributes = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

            // If there's an attribute, return its Name property
            if (attributes.Length > 0)
            {
                var displayAttribute = (DisplayAttribute)attributes[0];
                return displayAttribute.Name;
            }
        }

        // If there's no Display attribute, fall back to the enum value's literal name
        return enumValue.ToString();
    }
}
