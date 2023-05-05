using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ReactAsp.Data.Schedule;

public class Group
{
    [Key] public int Id { get; set; }
    public string GroupNumber { get; set; }

    public ICollection<Lesson> Lessons { get; set; }
    [JsonIgnore] public ICollection<LessonGroup> LessonClasses { get; set; }
}

public sealed class Teacher
{
    [Key] public int Id { get; set; }
    public string FullName { get; set; }

    public ICollection<Lesson> Lessons { get; set; }
}

public sealed class Subject
{
    [Key] public int Id { get; set; }
    public string SubjectName { get; set; }
    public ICollection<Lesson> Lessons { get; set; }
}

public sealed class Classroom
{
    [Key] public int Id { get; set; }
    public string ClassroomNumber { get; set; }
    public ICollection<Lesson> Lessons { get; set; }
}

public sealed class Lesson
{
    [Key] public int Id { get; set; }
    public int SubjectId { get; set; }
    public string DayOfWeek { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }

    public int ClassroomId { get; set; }
    public int TeacherId { get; set; }
    public int ScheduleLoadId { get; set; }
    public int WeekType { get; set; }

    [ForeignKey("SubjectId")] public Subject Subject { get; set; }

    [ForeignKey("ClassroomId")] public Classroom Classroom { get; set; }

    [ForeignKey("TeacherId")] public Teacher Teacher { get; set; }

    [ForeignKey("ScheduleLoadId")] public ScheduleLoad ScheduleLoad { get; set; }

    [JsonIgnore] public ICollection<LessonGroup> LessonGroups { get; set; }
}

public sealed class LessonGroup
{
    [Key] public int Id { get; set; }
    public int LessonId { get; set; }
    public int GroupId { get; set; }

    [ForeignKey("LessonId")] [JsonIgnore] public Lesson Lesson { get; set; }

    [ForeignKey("GroupId")] [JsonIgnore] public Group Group { get; set; }
}

public sealed class ScheduleLoad
{
    [Key] public int Id { get; set; }
    public DateTime LoadDate { get; set; }

    [JsonIgnore] public ICollection<Lesson> Lessons { get; set; }
}
