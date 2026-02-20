using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeerFit.Core.Models;

public class Member
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string FirstName  { get; set; } = null!;
    public string LastName   { get; set; } = null!;
    public string Email      { get; set; } = null!;
    public string Phone      { get; set; } = string.Empty;

    public DateTime   MemberSince { get; set; } = DateTime.UtcNow;
    public MembershipType Membership { get; set; } = MembershipType.Basic;
    public bool       IsActive   { get; set; } = true;

    [BsonIgnore]
    public string FullName => $"{FirstName} {LastName}";
}

public enum MembershipType
{
    Basic,
    Premium
}