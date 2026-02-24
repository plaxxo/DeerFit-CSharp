using DeerFit.Core.Repositories;
using DeerFit.Core.Services;
using DeerFit.Core.Settings;
using DeerFit.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// MongoDB - Support both appsettings.json and Environment Variables
builder.Services.Configure<MongoDbSettings>(options =>
{
    // Try environment variables first (for Docker/Render)
    var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING")
                          ?? builder.Configuration["MongoDbSettings:ConnectionString"];
    var databaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME")
                      ?? builder.Configuration["MongoDbSettings:DatabaseName"];

    options.ConnectionString = connectionString ?? throw new InvalidOperationException("MongoDB ConnectionString is not configured");
    options.DatabaseName = databaseName ?? throw new InvalidOperationException("MongoDB DatabaseName is not configured");
    options.MembersCollection = "members";
    options.CoursesCollection = "courses";
    options.BookingsCollection = "bookings";
});

// Repositories – Singleton, da MongoClient thread-safe ist
builder.Services.AddSingleton<MemberRepository>();
builder.Services.AddSingleton<CourseRepository>();
builder.Services.AddSingleton<BookingRepository>();

// Services
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<CourseService>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Don't redirect to HTTPS in Docker
// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Blazor Web App Endpunkt (nicht mehr MapBlazorHub + MapFallbackToPage)
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();