using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactAsp.Data.Schedule;

namespace ReactAsp.Controllers
{
    [ApiController]
    [Route("teachers")]
    public class TeacherController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public TeacherController(ScheduleContext context)
        {
            _context = context;
        }

        // GET: api/Teacher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTeachers()
        {
            return await _context.Teacher
                .Select(l => new
                {
                    l.Id,
                    l.FullName
                })
                .ToListAsync();
        }
        
        
        // GET: api/Teacher/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTeacher(int id)
        {
            var classroom = await _context.Teacher
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.FullName,
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
                            l.Classroom.ClassroomNumber,
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


        // PUT: api/Teacher/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher lesson)
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
                if (!TeacherExists(id))
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

        // POST: api/Teacher
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(Teacher lesson)
        {
            _context.Teacher.Add(lesson);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeacher), new { id = lesson.Id }, lesson);
        }

        // DELETE: api/Teacher/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var lesson = await _context.Teacher.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            _context.Teacher.Remove(lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.Id == id);
        }
    }
}
