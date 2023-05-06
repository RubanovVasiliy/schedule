using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactAsp.Data.Schedule;

namespace ReactAsp.Controllers;

[ApiController]
[Route("groups")]
public class GroupsController : ControllerBase
{
    private readonly ScheduleContext _context;

    public GroupsController(ScheduleContext context)
    {
        _context = context;
    }

    // GET: api/Groups
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetGroups()
    {
        return await _context.Groups
            .Select(l => new
            {
                l.Id,
                l.GroupNumber
            })
            .ToListAsync();
    }


    // GET: api/Groups/5
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetGroups(int id)
    {
        var group = await _context.Groups
            .Include(g => g.LessonClasses)
            .ThenInclude(lg => lg.Lesson)
            .ThenInclude(l => l.Subject)
            .Include(g => g.LessonClasses)
            .ThenInclude(lg => lg.Lesson)
            .ThenInclude(l => l.Classroom)
            .Include(g => g.LessonClasses)
            .ThenInclude(lg => lg.Lesson)
            .ThenInclude(l => l.Teacher)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group == null)
        {
            return NotFound();
        }

        var lessons = group.LessonClasses.Select(lg => lg.Lesson);

        return new
        {
            group.Id,
            group.GroupNumber,
            Lessons = lessons.Select(l => new
            {
                l.Id,
                l.Subject.SubjectName,
                l.DayOfWeek,
                l.StartTime,
                l.EndTime,
                l.Classroom.ClassroomNumber,
                TeacherFullName = l.Teacher.FullName,
                l.WeekType
            }).ToList()
        };
    }


    // PUT: api/Groups/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGroups(int id, Group group)
    {
        if (id != group.Id)
        {
            return BadRequest();
        }

        _context.Entry(group).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GroupsExists(id))
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

    // POST: api/Groups
    [HttpPost]
    public async Task<ActionResult<Group>> PostGroups(Group group)
    {
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGroups), new { id = group.Id }, group);
    }

    // DELETE: api/Groups/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroups(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
        {
            return NotFound();
        }

        _context.Groups.Remove(group);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool GroupsExists(int id)
    {
        return _context.Groups.Any(e => e.Id == id);
    }
}