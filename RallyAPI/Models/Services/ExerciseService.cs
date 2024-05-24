using MongoDB.Driver;
using RallyAPI.Data;

namespace RallyAPI.Models.Services;

public class ExerciseService(IDbAccess<Exercise> dbAccess)
{
    private readonly IMongoCollection<Exercise> _exercises = dbAccess.GetDbClient("Exercise");

    public IEnumerable<Exercise> GetAllExercises()
    {
        var query = _exercises.AsQueryable();

        return query;
    }
}
