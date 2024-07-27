using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskApp.Models;

record TodoItem(string Task, DateTime TimeStamp, TodoList List)
{
    public bool IsDone { get; set; }

    public static List<TodoItem> All { get; } = SetupTodoItems();

    public static List<TodoItem> SetupTodoItems()
    {
        var todoItems = new List<TodoItem>();

        var taskDescriptions = new List<string>
        {
            "Read a book",
            "Go for a run",
            "Meet with client",
            "Write a blog post",
            "Plan the weekend trip",
            "Organize the desk",
            "Water the plants",
            "Finish the presentation",
            "Clean the garage",
            "Prepare dinner",
            "Call parents",
            "Attend yoga class",
            "Buy groceries",
            "Fix the bike",
            "Complete project report",
            "Journal daily thoughts",
            "Practice guitar",
            "Meditate",
            "Work on side project",
            "Update LinkedIn profile",
            "Go hiking",
            "Prepare for meeting",
            "Research new kitchen recipes",
            "Catch up with emails",
            "Learn a new language lesson",
            "Pay bills",
            "Schedule doctor appointment",
            "Watch a documentary",
            "Organize a game night with friends",
            "Sort out old photos"
        };

        for (int i = 0; i < 300; i++)
        {
            string task = taskDescriptions[Random.Shared.Next(0, 29)];
            var list = (TodoList)Random.Shared.Next(Enum.GetNames(typeof(TodoList)).Length);
            var todoItem = new TodoItem(task, DateTime.Today.AddDays(-10).AddHours(i * 3), list)
            {
                IsDone = Random.Shared.NextDouble() < 0.7
            };

            todoItems.Add(todoItem);
        }

        return todoItems;
    }
}

class TodoItemGroup : List<TodoItem>
{
    private TodoItemGroup(DateTime date, List<TodoItem> items)
        : base(items)
    {
        Date = date;
    }

    public DateTime Date { get; }

    public static List<TodoItemGroup> All { get; private set; } = CreateGroups();

    private static List<TodoItemGroup> CreateGroups()
    {
        return TodoItem
            .All
            .GroupBy(_ => _.TimeStamp.Date)
            .Select(_ => new TodoItemGroup(_.Key, [.. _]))
            .ToList();
    }

    public static void AddItem(TodoItem item)
    {
        TodoItem.All.Add(item);
        All = CreateGroups();
    }
}



enum TodoList
{
    None,
    Hobby,
    Personal,
    LifeStyle,
    Work
}
