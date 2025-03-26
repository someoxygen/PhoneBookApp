using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContactService.Domain.Entities;

public class ContactInformation
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();

    [BsonRepresentation(BsonType.String)]
    public Guid PersonId { get; set; }

    public ContactType Type { get; set; }
    public string Content { get; set; } = null!;
}

public enum ContactType
{
    PhoneNumber,
    EmailAddress,
    Location
}
