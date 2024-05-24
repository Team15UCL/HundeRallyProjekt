using MongoDB.Bson;

namespace RallyAPI.Models;

public class Exercise
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public string Class { get; set; }
    public string Url { get; set; }
}
