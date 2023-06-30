namespace Chateo.Server.Models;


class User
{
    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public Guid? TypingToUserId { get; set; }

    public static List<User> All { get; } = new();
}

public class Message
{
    public Guid Id { get; set; }

    public Guid FromUserId { get; set; }

    public Guid ToUserId { get; set; }

    public required string Content { get; set; }

    public required DateTime TimeStamp { get; set; }

    public bool Seen { get; set; }

    public static List<Message> All { get; } = new();
}

