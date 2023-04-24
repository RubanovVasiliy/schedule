using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReactAsp.Data.Schedule;

namespace ReactAsp.Controllers
{
    [ApiController]
    [Route("classroom")]
    public class ClassroomController : ControllerBase
    {
        private readonly ScheduleContext _context;

        public ClassroomController(ScheduleContext context)
        {
            _context = context;
        }

        // GET: api/Classroom
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Classroom>>> GetClassrooms()
        {
            return await _context.Classrooms
                .Include(i => i.Lessons)
                .ToListAsync();
        }
        

        // GET: api/Classroom/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Classroom>> GetClassroom(int id)
        {
            var lesson = await _context.Classrooms.FindAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            return lesson;
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
