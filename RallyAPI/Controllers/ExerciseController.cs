using Microsoft.AspNetCore.Mvc;
using RallyAPI.Models;
using RallyAPI.Models.Services;

namespace RallyAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class ExerciseController(ExerciseService exerciseService) : ControllerBase
{
    [HttpGet]
    public IEnumerable<Exercise> GetExercises()
    {
        return exerciseService.GetAllExercises();
    }
}
