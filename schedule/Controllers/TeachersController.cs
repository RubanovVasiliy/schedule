using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using schedule.Dtos;
using schedule.Models;

namespace schedule.Controllers;


[ApiController]
[Route("teachers")]
public class TeachersController : ControllerBase
{
    private readonly MyDbContext _context;

    public TeachersController(MyDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("getAll")]
    public async Task<ActionResult<List<Teacher>>> GetAll()
    {
        var books = await _context.Teachers.ToListAsync();
        return Ok(books);
    }

    [HttpGet]
    [Route("getById/{id:int}")]
    public async Task<ActionResult<Teacher>> GetById(int id)
    {
        var book = await _context.Teachers.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPost]
    [Route("add")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Teacher>> Add(CreateTeacherDto dto)
    {
        var book = new Teacher()
        {
            Fullname = dto.Fullname
        };
        await _context.Teachers.AddAsync(book);
        await _context.SaveChangesAsync();
        return Ok(book);
    }

    [HttpPut]
    [Route("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Teacher>> Update(Teacher updatedBook)
    {
        var book = await _context.Teachers.FindAsync(updatedBook.Id);
        if (book == null)
        {
            return NotFound();
        }

        book.Fullname = updatedBook.Fullname;
        await _context.SaveChangesAsync();
        return Ok(updatedBook);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Teacher>> Delete(int id)
    {
        var book = await _context.Teachers.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        _context.Teachers.Remove(book);
        await _context.SaveChangesAsync();

        return Ok(book);
    }
}