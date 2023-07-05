namespace Chateo.Server.Models;


public class User
{
    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Avatar { get; set; }

    public Guid? TypingToUserId { get; set; }

    public DateTime LastSeen { get; set; }


    public static List<User> All { get; } = new();
}

