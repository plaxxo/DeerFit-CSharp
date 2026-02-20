using DeerFit.Core.Models;
using DeerFit.Core.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DeerFit.Core.Repositories;

public class CourseRepository : BaseRepository<Course>
{
    public CourseRepository(IOptions<MongoDbSettings> settings)
        : base(settings, settings.Value.CoursesCollection) { }

    public async Task<List<Course>> GetUpcomingAsync() =>
        await Collection
            .Find(c => c.StartTime > DateTime.UtcNow)
            .SortBy(c => c.StartTime)
            .ToListAsync();

    public async Task<List<Course>> GetByTrainerAsync(string trainer) =>
        await Collection.Find(c => c.Trainer == trainer).ToListAsync();

    public async Task AddMemberToBookedListAsync(string courseId, string memberId)
    {
        var update = Builders<Course>.Update.AddToSet(c => c.BookedMemberIds, memberId);
        await Collection.UpdateOneAsync(c => c.Id == courseId, update);
    }

    public async Task RemoveMemberFromBookedListAsync(string courseId, string memberId)
    {
        var update = Builders<Course>.Update.Pull(c => c.BookedMemberIds, memberId);
        await Collection.UpdateOneAsync(c => c.Id == courseId, update);
    }
}