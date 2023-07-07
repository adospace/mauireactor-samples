namespace Chateo.Shared;

public record MessageCreateModel(Guid Id, Guid FromUserId, Guid ToUserId, string Content);

public record MessageUpdateModel(Guid Id);

public record MessageViewModel(Guid Id, Guid FromUserId, Guid ToUserId, string Content, DateTime TimeStamp, DateTime? ReadTimeStamp);
