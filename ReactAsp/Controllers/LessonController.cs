using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactAsp.Data.Schedule;

namespace ReactAsp.Controllers
{
    [ApiController]
    [Route("lesson")]
    public class LessonController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public LessonController(ScheduleContext context)
        {
            _context = context;
        }

        // GET: api/Lesson
        [HttpGet]
        [SuppressMessage("ReSharper.DPA", "DPA0007: Large number of DB records", MessageId = "count: 109")]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetLessons()
        {
            return await _context.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Subject)
                .Include(l => l.Classroom)
                .Select(l => new Lesson {
                    Id = l.Id,
                    DayOfWeek = l.DayOfWeek,
                    StartTime = l.StartTime,
                    EndTime = l.EndTime,
                    Teacher = new Teacher {
                        FullName = l.Teacher.FullName
                    },
                    Subject = new Subject {
                        SubjectName = l.Subject.SubjectName
                    },
                    Classroom = new Classroom {
                        ClassroomNumber = l.Classroom.ClassroomNumber
                    },
                })
                .ToListAsync();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetLessons(string name)
        {
            return await _context.Lessons
                .Include(i => i.Subject)
                .Include(i => i.Classroom)
                .Include(i => i.Teacher)
                .Where(i => i.Teacher.FullName == name)
                .ToListAsync();
        }

        // GET: api/Lesson/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lesson>> GetLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            return lesson;
        }

        // PUT: api/Lesson/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLesson(int id, Lesson lesson)
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
                if (!LessonExists(id))
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

        // POST: api/Lesson
        [HttpPost]
        public async Task<ActionResult<Lesson>> PostLesson(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lesson);
        }

        // DELETE: api/Lesson/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.Id == id);
        }
    }
}
