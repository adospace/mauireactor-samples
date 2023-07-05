namespace Chateo.Shared;

public record UserCreateModel(Guid Id, string FirstName, string LastName, string Avatar);

public record UserViewModel(Guid Id, string FirstName, string LastName, string Avatar, DateTime LastSeen);

public record UserUpdatedModel(Guid Id, Guid? TypingToUserId, DateTime LastSeen);


