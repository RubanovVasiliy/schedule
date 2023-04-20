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
            
            
            var groupRepository = new GroupRepository(_context);
            var teacherRepository = new TeacherRepository(_context);
            var subjectRepository = new SubjectRepository(_context);
            var classroomRepository = new ClassroomRepository(_context);
            var lessonRepository = new LessonRepository(_context);
            var groupOnClassRepository = new GroupOnClassRepository(_context);
            var scheduleLoadRepository = new ScheduleLoadRepository(_context);
            
            await scheduleLoadRepository.CreateAsync(new ScheduleLoad { LoadDate = DateTime.Now.ToUniversalTime() });
            foreach (var subject in result.Subjects)
            {
                await subjectRepository.CreateIfNotExistAsync(new Subject { SubjectName = subject });
            }

            foreach (var group in result.Groups)
            {
                await groupRepository.CreateIfNotExistAsync(new Group { GroupNumber = group });
            }

            
            foreach (var classroom in result.Classrooms)
            {
                await classroomRepository.CreateIfNotExistAsync(new Classroom { ClassroomNumber = classroom });
            }


            foreach (var teacherSchedule in result.Schedule)
            {
                var teacher = teacherSchedule.TeacherName;
                await teacherRepository.CreateIfNotExistAsync(new Teacher { FullName = teacher });

                foreach (var lesson in teacherSchedule.Lessons)
                {
                    var parts = lesson.Split('#');
                    
                    var classroom = "";
                    if (parts.Length == 7)
                    {
                        classroom = parts[5];
                    }
                    
                    var subject = parts[3];
                    var groups = parts[4].Split(", ");



                    /*await lessonRepository.CreateAsync(new Lesson
                    {
                    });*/
                }
            }
            
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
}