using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Models;

[Route("api/[controller]")]
[ApiController]
public class ActivitiesController : ControllerBase
{
    private readonly TrainingAppContext _context;

    public ActivitiesController(TrainingAppContext context)
    {
        _context = context;
    }

    // GET: api/activities
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Activity>>> GetActivity()
    {
        return await _context.Activities.ToListAsync();
    }

    // GET: api/activities/details - использует AllActivities (без ExerciseId)
    [HttpGet("details")]
    public async Task<ActionResult<IEnumerable<AllActivity>>> GetActivitiesWithDetails()
    {
        var activities = await _context.AllActivities
            .OrderByDescending(a => a.DateActivity)
            .ToListAsync();

        return Ok(activities);
    }

    // GET: api/activities/{activityid}
    [HttpGet("{activityid}")]
    public async Task<ActionResult<Activity>> GetActivity(Guid activityid)
    {
        var activity = await _context.Activities.FindAsync(activityid);

        if (activity == null)
        {
            return NotFound($"Активность с ID {activityid} не найдена");
        }

        return activity;
    }

    // GET: api/activities/by-date?date=2024-01-01
    [HttpGet("by-date")]
    public async Task<ActionResult<IEnumerable<AllActivity>>> GetActivitiesByDate([FromQuery] DateTime date)
    {
        var dateOnly = DateOnly.FromDateTime(date);

        var activities = await _context.AllActivities
            .Where(a => a.DateActivity == dateOnly)
            .OrderBy(a => a.DateActivity)
            .ToListAsync();

        return Ok(activities);
    }

    // GET: api/activities/exercise/{exerciseId} - ИСПРАВЛЕНО: использует Activity
    [HttpGet("exercise/{exerciseId}")]
    public async Task<ActionResult<IEnumerable<Activity>>> GetActivitiesByExercise(Guid exerciseId)
    {
        var activities = await _context.Activities
            .Where(a => a.ExerciseId == exerciseId)
            .OrderByDescending(a => a.DateActivity)
            .ToListAsync();

        return Ok(activities);
    }

    // PUT: api/activities/{activityid}
    [HttpPut("{activityid}")]
    public async Task<IActionResult> PutActivity(Guid activityid, Activity activity)
    {
        if (activityid != activity.ActivityId)
        {
            return BadRequest("ID в URL не совпадает с ID в теле запроса");
        }

        var existingActivity = await _context.Activities.FindAsync(activityid);
        if (existingActivity == null)
        {
            return NotFound($"Активность с ID {activityid} не найдена");
        }

        // Обновляем только разрешенные поля
        existingActivity.ExerciseId = activity.ExerciseId;
        existingActivity.DateActivity = activity.DateActivity;
        existingActivity.Duration = activity.Duration;
        existingActivity.Note = activity.Note;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ActivityExists(activityid))
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

    // POST: api/activities
    [HttpPost]
    public async Task<ActionResult<Activity>> PostActivity(Activity activity)
    {
        // Проверяем ExerciseId
        if (activity.ExerciseId == null || activity.ExerciseId == Guid.Empty)
        {
            return BadRequest("Необходимо указать ExerciseId");
        }

        // Проверяем существование упражнения
        var exerciseExists = await _context.Exercises
            .AnyAsync(e => e.ExerciseId == activity.ExerciseId);

        if (!exerciseExists)
        {
            return BadRequest($"Упражнение с ID {activity.ExerciseId} не найдено");
        }

        // Генерируем ID
        if (activity.ActivityId == Guid.Empty)
        {
            activity.ActivityId = Guid.NewGuid();
        }

        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetActivity), new { activityid = activity.ActivityId }, activity);
    }

    // DELETE: api/activities/{activityid}
    [HttpDelete("{activityid}")]
    public async Task<IActionResult> DeleteActivity(Guid activityid)
    {
        var activity = await _context.Activities.FindAsync(activityid);
        if (activity == null)
        {
            return NotFound($"Активность с ID {activityid} не найдена");
        }

        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ActivityExists(Guid? activityid)
    {
        return _context.Activities.Any(e => e.ActivityId == activityid);
    }
}