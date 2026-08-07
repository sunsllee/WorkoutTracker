using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Models;

[Route("api/[controller]")]
[ApiController]
public class ExercisesController : ControllerBase
{
    private readonly TrainingAppContext _context;
    public ExercisesController(TrainingAppContext context)
    {
        _context = context;
    }

    // GET: api/Exercise
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Exercise>>> GetExercise()
    {
        return await _context.Exercises.ToListAsync();
    }

    // GET: api/Exercise/5
    [HttpGet("{exerciseid}")]
    public async Task<ActionResult<Exercise>> GetExercise(System.Guid exerciseid)
    {
        var exercise = await _context.Exercises.FindAsync(exerciseid);

        if (exercise == null)
        {
            return NotFound();
        }

        return exercise;
    }

    // PUT: api/Exercise/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{exerciseid}")]
    public async Task<IActionResult> PutExercise(System.Guid? exerciseid, Exercise exercise)
    {
        if (exerciseid != exercise.ExerciseId)
        {
            return BadRequest();
        }

        _context.Entry(exercise).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ExerciseExists(exerciseid))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Exercise
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Exercise>> PostExercise(Exercise exercise)
    {
        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetExercise", new { exerciseid = exercise.ExerciseId }, exercise);
    }

    // DELETE: api/Exercise/5
    [HttpDelete("{exerciseid}")]
    public async Task<IActionResult> DeleteExercise(System.Guid? exerciseid)
    {
        var exercise = await _context.Exercises.FindAsync(exerciseid);
        if (exercise == null)
        {
            return NotFound();
        }

        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ExerciseExists(System.Guid? exerciseid)
    {
        return _context.Exercises.Any(e => e.ExerciseId == exerciseid);
    }
}
