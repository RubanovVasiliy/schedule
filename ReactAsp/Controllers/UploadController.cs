using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ReactAsp.Data;
using ReactAsp.Data.Schedule;
using ReactAsp.Data.Schedule.Repository;
using ReactAsp.Utils;

namespace ReactAsp.Controllers;

[ApiController]
[Route("upload")]
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


            // init load date
            var scheduleLoadRepository = new ScheduleLoadRepository(_context);
            var scheduleLoadId = (
                await scheduleLoadRepository.CreateAsync(
                    new ScheduleLoad
                    {
                        LoadDate = DateTime.Now.ToUniversalTime()
                    })
            ).Id;


            // init load date
            var subjectRepository = new SubjectRepository(_context);
            foreach (var subject in result.Subjects)
            {
                await subjectRepository.CreateIfNotExistAsync(new Subject { SubjectName = subject });
            }

            // init load groups
            var groupRepository = new GroupRepository(_context);
            foreach (var group in result.Groups)
            {
                await groupRepository.CreateIfNotExistAsync(new Group { GroupNumber = group });
            }


            // init load classrooms
            var classroomRepository = new ClassroomRepository(_context);
            foreach (var classroom in result.Classrooms)
            {
                await classroomRepository.CreateIfNotExistAsync(new Classroom { ClassroomNumber = classroom });
            }

            await classroomRepository.CreateIfNotExistAsync(new Classroom { ClassroomNumber = "" });


            // init load teachers and lessons
            var teacherRepository = new TeacherRepository(_context);
            var lessonRepository = new LessonRepository(_context);
            var lessonClassRepository = new LessonClassRepository(_context);


            foreach (var teacherSchedule in result.Schedule)
            {
                var teacher = teacherSchedule.TeacherName;
                await teacherRepository.CreateIfNotExistAsync(new Teacher { FullName = teacher });
                var teacherId = (await teacherRepository.GetByFieldValueAsync(e => e.FullName == teacher)).Id;


                foreach (var lesson in teacherSchedule.Lessons)
                {
                    var parts = lesson.Split('#');

                    var classroom = "";
                    if (parts.Length == 7) classroom = parts[5];

                    var dayOfWeek = parts[0];
                    var startTime = parts[1];
                    var endTime = parts[2];
                    var subject = parts[3];
                    var groups = parts[4].Split(", ");
                    var isOddWeek = parts[^1].Equals("0");

                    int classroomId;
                    if (!classroom.Equals(""))
                    {
                        var res = await classroomRepository.GetByFieldValueAsync(e => e.ClassroomNumber == classroom);
                        classroomId = res.Id;
                    }
                    else
                    {
                        classroomId =
                            (await classroomRepository.GetByFieldValueAsync(e => e.ClassroomNumber == classroom)).Id;
                    }

                    var subjectId = (await subjectRepository.GetByFieldValueAsync(e => e.SubjectName == subject)).Id;

                    var lessonId = (await lessonRepository.CreateAsync(new Lesson
                    {
                        DayOfWeek = dayOfWeek,
                        StartTime = startTime,
                        EndTime = endTime,
                        SubjectId = subjectId,
                        TeacherId = teacherId,
                        IsOddWeek = isOddWeek,
                        ScheduleLoadId = scheduleLoadId,
                        ClassroomId = classroomId
                    })).Id;


                    foreach (var group in groups)
                    {
                        var groupId = (await groupRepository.GetByFieldValueAsync(e => e.GroupNumber == group)).Id;
                        await lessonClassRepository.CreateAsync(new LessonClass()
                            { GroupId = groupId, LessonId = lessonId });
                    }

                }
            }

            return Ok(new { message = "File uploaded successfully" });
        }

        catch (Exception ex)
        {
            return BadRequest(new { message = $"Error: {ex.Message}" });
        }

    }
}