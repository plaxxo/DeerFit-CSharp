using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeerFit.Core.Models;

public class Booking
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string MemberId { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string CourseId { get; set; } = null!;

    public DateTime      BookedAt { get; set; } = DateTime.UtcNow;
    public BookingStatus Status   { get; set; } = BookingStatus.Confirmed;
}

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    Attended
}