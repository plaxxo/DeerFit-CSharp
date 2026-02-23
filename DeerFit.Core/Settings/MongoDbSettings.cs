namespace DeerFit.Core.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName     { get; set; } = null!;
    
    public string MembersCollection  { get; set; } = "members";
    public string CoursesCollection  { get; set; } = "courses";
    public string BookingsCollection { get; set; } = "bookings";
}