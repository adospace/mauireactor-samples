namespace Chateo.Server.Models;

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

