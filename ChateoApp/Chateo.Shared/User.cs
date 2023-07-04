namespace Chateo.Shared;

public record UserCreateModel(Guid Id, string FirstName, string LastName);

public record UserViewModel(Guid Id, string FirstName, string LastName, DateTime LastSeen);

public record UserUpdatedModel(Guid Id, Guid? TypingToUserId, DateTime LastSeen);
