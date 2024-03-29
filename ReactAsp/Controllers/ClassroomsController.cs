using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ReactAsp.Data.Schedule;

namespace ReactAsp.Controllers
{
    [ApiController]
    [Route("classrooms")]
    public class ClassroomsController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public ClassroomsController(ScheduleContext context)
        {
            _context = context;
        }

        // GET: api/Classroom
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetClassrooms()
        {
            return await _context.Classrooms
                .Select(l => new
                {
                    l.Id,
                    l.ClassroomNumber
                })
                .ToListAsync();
        }
        
        
        // GET: api/Classroom/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetClassroom(int id)
        {
            var classroom = await _context.Classrooms
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.ClassroomNumber,
                    Lessons = c.Lessons
                        .OrderBy(l => l.DayOfWeek)
                        .ThenBy(l => l.StartTime)
                        .Select(l => new
                        {
                            l.Id, 
                            l.DayOfWeek,
                            l.StartTime, 
                            l.EndTime,
                            l.WeekType,
                            l.Subject.SubjectName,
                            l.Teacher.FullName,
                            groups = l.LessonsGroups.Select(g=>g.Group.GroupNumber)
                        })
                        .ToList()
                })
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            if (classroom == null)
            {
                return NotFound();
            }

            return classroom;
        }


        // PUT: api/Classroom/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClassroom(int id, Classroom lesson)
        {
            if (id != lesson.Id)
            {
                return BadRequest();
            }

            _context.Entry(lesson).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassroomExists(id))
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

        // POST: api/Classroom
        [HttpPost]
        public async Task<ActionResult<Classroom>> PostClassroom(Classroom lesson)
        {
            _context.Classrooms.Add(lesson);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClassroom), new { id = lesson.Id }, lesson);
        }

        // DELETE: api/Classroom/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            var lesson = await _context.Classrooms.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            _context.Classrooms.Remove(lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassroomExists(int id)
        {
            return _context.Classrooms.Any(e => e.Id == id);
        }
    }
}
