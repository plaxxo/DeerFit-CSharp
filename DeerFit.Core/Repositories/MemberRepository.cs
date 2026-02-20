using DeerFit.Core.Models;
using DeerFit.Core.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DeerFit.Core.Repositories;

public class MemberRepository : BaseRepository<Member>
{
    public MemberRepository(IOptions<MongoDbSettings> settings)
        : base(settings, settings.Value.MembersCollection) { }

    public async Task<Member?> GetByEmailAsync(string email) =>
        await Collection.Find(m => m.Email == email).FirstOrDefaultAsync();

    public async Task<List<Member>> GetActiveAsync() =>
        await Collection.Find(m => m.IsActive).ToListAsync();

    public async Task<List<Member>> SearchAsync(string searchTerm) =>
        await Collection
            .Find(m => m.FirstName.Contains(searchTerm) ||
                       m.LastName.Contains(searchTerm)  ||
                       m.Email.Contains(searchTerm))
            .ToListAsync();
}