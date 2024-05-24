using MongoDB.Driver;

namespace RallyAPI.Data;
public interface IDbAccess<T> where T : class
{
    IMongoCollection<T> GetDbClient(string name);
}