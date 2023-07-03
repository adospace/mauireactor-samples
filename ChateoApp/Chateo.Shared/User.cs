namespace Chateo.Shared;

public record UserCreateModel(Guid Id, string FirstName, string LastName);

public record UserViewModel(Guid Id, string FirstName, string LastName);

public record UserUpdatedModel(Guid Id, Guid? TypingToUserId);

public record MessageCreateModel(Guid Id, Guid FromUserId, Guid ToUserId, string Content);
