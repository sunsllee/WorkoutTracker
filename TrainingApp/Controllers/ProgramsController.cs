using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingApp.Models;

[Route("api/[controller]")]
[ApiController]
public class ProgramsController : ControllerBase
{
    private readonly TrainingAppContext _context;
    public ProgramsController(TrainingAppContext context)
    {
        _context = context;
    }

    // GET: api/Program
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Program>>> GetProgram()
    {
        return await _context.Programs.ToListAsync();
    }

    // GET: api/Program/5
    [HttpGet("{programid}")]
    public async Task<ActionResult<Program>> GetProgram(System.Guid programid)
    {
        var program = await _context.Programs.FindAsync(programid);

        if (program == null)
        {
            return NotFound();
        }

        return program;
    }

    // PUT: api/Program/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{programid}")]
    public async Task<IActionResult> PutProgram(System.Guid? programid, Program program)
    {
        if (programid != program.ProgramId)
        {
            return BadRequest();
        }

        _context.Entry(program).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProgramExists(programid))
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

    // POST: api/Program
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Program>> PostProgram(Program program)
    {
        _context.Programs.Add(program);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetProgram", new { programid = program.ProgramId }, program);
    }

    // DELETE: api/Program/5
    [HttpDelete("{programid}")]
    public async Task<IActionResult> DeleteProgram(System.Guid? programid)
    {
        var program = await _context.Programs.FindAsync(programid);
        if (program == null)
        {
            return NotFound();
        }

        _context.Programs.Remove(program);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProgramExists(System.Guid? programid)
    {
        return _context.Programs.Any(e => e.ProgramId == programid);
    }
}
