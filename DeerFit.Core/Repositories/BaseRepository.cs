using DeerFit.Core.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DeerFit.Core.Repositories;

public abstract class BaseRepository<T> : IRepository<T>
{
    protected readonly IMongoCollection<T> Collection;

    protected BaseRepository(IOptions<MongoDbSettings> settings, string collectionName)
    {
        var client    = new MongoClient(settings.Value.ConnectionString);
        var database  = client.GetDatabase(settings.Value.DatabaseName);
        Collection    = database.GetCollection<T>(collectionName);
    }

    public async Task<List<T>> GetAllAsync() =>
        await Collection.Find(_ => true).ToListAsync();

    public async Task<T?> GetByIdAsync(string id) =>
        await Collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();

    public async Task CreateAsync(T entity) =>
        await Collection.InsertOneAsync(entity);

    public async Task UpdateAsync(string id, T entity) =>
        await Collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", id), entity);

    public async Task DeleteAsync(string id) =>
        await Collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", id));
}