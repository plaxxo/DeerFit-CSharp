using DeerFit.Core.Models;
using DeerFit.Core.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DeerFit.Core.Repositories;

public class BookingRepository : BaseRepository<Booking>
{
    public BookingRepository(IOptions<MongoDbSettings> settings)
        : base(settings, settings.Value.BookingsCollection) { }

    public async Task<List<Booking>> GetByMemberAsync(string memberId) =>
        await Collection.Find(b => b.MemberId == memberId).ToListAsync();

    public async Task<Booking?> GetExistingAsync(string memberId, string courseId) =>
        await Collection
            .Find(b => b.MemberId == memberId &&
                       b.CourseId == courseId &&
                       b.Status   == BookingStatus.Confirmed)
            .FirstOrDefaultAsync();

    public async Task UpdateStatusAsync(string id, BookingStatus status)
    {
        var update = Builders<Booking>.Update.Set(b => b.Status, status);
        await Collection.UpdateOneAsync(b => b.Id == id, update);
    }
}