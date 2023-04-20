using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using ReactAsp.Data.Schedule;
using ReactAsp.Data.Schedule.Repository;
using ReactAsp.Utils;

namespace ReactAsp.Controllers;

[ApiController]
[Route("[controller]")]
public class UploadController : ControllerBase
{
    private readonly ScheduleContext _context;

    public UploadController(ScheduleContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> Upload()
    {
        try
        {
            if (Request.Form.Files.Count == 0)
            {
                return BadRequest(new { message = "File not found" });
            }

            var file = Request.Form.Files[0];

            if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return BadRequest(new { message = "Invalid file format. Please upload an Excel file" });
            }

            var filePath = Path.GetTempFileName();

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileInfo = new FileInfo(filePath);
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage(fileInfo);
            var worksheet = package.Workbook.Worksheets["TDSheet"];

            var parser = new ExcelParser(worksheet);
            var result = parser.ParseData();

            foreach (var teacherSchedule in result)
            {
                Console.WriteLine(teacherSchedule.TeacherName);
                foreach (var lesson in teacherSchedule.Lessons)
                {
                    Console.WriteLine(lesson);
                }
            }
            
            var groupRepository = new GroupRepository(_context);
            var newGroup = new Group { GroupNumber =  Guid.NewGuid().ToString() };
            
            await groupRepository.CreateAsync(newGroup);
            await _context.SaveChangesAsync();

            var all = await groupRepository.GetAllAsync();
            
            return Ok(all);

            //Console.WriteLine(result);

            return Ok(new { message = "File uploaded successfully" });
        }

        catch (Exception ex)
        {
            return BadRequest(new { message = $"Error: {ex.Message}" });
        }
    }
    
    [HttpGet]
    [Route("groups")]
    public async Task<ActionResult<Group>> CreateGroup(int id)
    {
        var groupRepository = new GroupRepository(_context);
        var teacherRepository = new TeacherRepository(_context);
        var subjectRepository = new SubjectRepository(_context);
        var classroomRepository = new ClassroomRepository(_context);
        var lessonRepository = new LessonRepository(_context);
        var groupOnClassRepository = new GroupOnClassRepository(_context);
        var scheduleLoadRepository = new ScheduleLoadRepository(_context);
        
        // Create a new group
        var newGroup = new Group { GroupNumber = "GroupA" };
        await groupRepository.CreateAsync(newGroup);
        await _context.SaveChangesAsync();
        
        //return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);

        return Ok(id);
    }
}