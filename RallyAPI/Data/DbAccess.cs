using MongoDB.Driver;

namespace RallyAPI.Data;

public class DbAccess<T> : IDbAccess<T> where T : class
{

    readonly MongoClient client = new("mongodb+srv://askelysgaard:1234@cluster0.avn6de9.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");

    public IMongoCollection<T> GetDbClient(string name)
    {
        var db = client.GetDatabase("RallyBane").GetCollection<T>(name);
        return db;
    }

}
