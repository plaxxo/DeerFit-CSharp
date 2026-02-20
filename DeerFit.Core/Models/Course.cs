using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeerFit.Core.Models;

public class Course
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string   Name        { get; set; } = null!;
    public string   Description { get; set; } = string.Empty;
    public string   Trainer     { get; set; } = null!;
    public DateTime StartTime   { get; set; }
    public int      DurationMin { get; set; }  // Dauer in Minuten
    public int      MaxCapacity { get; set; }

    public List<string> BookedMemberIds { get; set; } = new();

    [BsonIgnore]
    public int FreeSpots => MaxCapacity - BookedMemberIds.Count;

    [BsonIgnore]
    public bool IsFull => FreeSpots <= 0;

    [BsonIgnore]
    public DateTime EndTime => StartTime.AddMinutes(DurationMin);
}