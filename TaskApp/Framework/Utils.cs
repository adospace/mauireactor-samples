using TaskApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskApp.Framework;

class Utils
{
    public static string GetHumanFriendlyDate(DateTime date)
    {
        DateTime today = DateTime.Today;

        if (date.Date == today)
        {
            return "Today";
        }
        else if (date.Date == today.AddDays(1))
        {
            return "Tomorrow";
        }
        else if (date.Date == today.AddDays(-1))
        {
            return "Yesterday";
        }
        else
        {
            return date.ToString("MMMM dd");
        }
    }


    public static Color GetColorForList(TodoList list)
    {
        return list switch
        {
            TodoList.Personal => Color.FromArgb("#037FFF"),
            TodoList.LifeStyle => Color.FromArgb("#00A86B"),
            TodoList.Work => Color.FromArgb("#F4AF25"),
            TodoList.Hobby => Color.FromArgb("#F44725"),
            TodoList.None => Color.FromArgb("#D3D9D9"),
            _ => throw new NotSupportedException(),
        };
    }

}
