namespace DeerFit.Core.Settings;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName     { get; set; } = null!;
    
    public string MembersCollection  { get; set; } = "Members";
    public string CoursesCollection  { get; set; } = "Courses";
    public string BookingsCollection { get; set; } = "Bookings";
}