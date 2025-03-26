using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContactService.Domain.Entities;

public class Person
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Company { get; set; }
}
