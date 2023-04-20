using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ReactAsp.Data.Schedule;

public class Group
{
    [Key]
    public int Id { get; set; }
    public string GroupNumber { get; set; }
    
    public ICollection<Lesson> Lessons { get; set; }
    public ICollection<GroupOnClass> GroupsOnClasses { get; set; }
}

public class Teacher
{
    [Key]
    public int Id { get; set; }
    public string FullName { get; set; }
    
    public virtual ICollection<Lesson> Lessons { get; set; }
}

public class Subject
{
    [Key]
    public int Id { get; set; }
    public string SubjectName { get; set; }
    
    public virtual ICollection<Lesson> Lessons { get; set; }
}

public class Classroom
{
    [Key]
    public int Id { get; set; }
    public string ClassroomNumber { get; set; }
    
    public virtual ICollection<Lesson> Lessons { get; set; }
}

public class Lesson
{
    [Key]
    public int Id { get; set; }
    public int SubjectId { get; set; }
    public string DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int ClassroomId { get; set; }
    public int TeacherId { get; set; }
    public int ScheduleLoadId { get; set; }
    public bool IsEvenWeek { get; set; }
    
    [ForeignKey("SubjectId")]
    public virtual Subject Subject { get; set; }
    
    [ForeignKey("ClassroomId")]
    public virtual Classroom Classroom { get; set; }
    
    [ForeignKey("TeacherId")]
    public virtual Teacher Teacher { get; set; }
    
    [ForeignKey("ScheduleLoadId")]
    public virtual ScheduleLoad ScheduleLoad { get; set; }
    
    public virtual ICollection<GroupOnClass> GroupsOnClasses { get; set; }
}

public class GroupOnClass
{
    [Key]
    public int Id { get; set; }
    public int ScheduleId { get; set; }
    public int GroupId { get; set; }
    
    [ForeignKey("ScheduleId")]
    public virtual Lesson Lesson { get; set; }
    
    [ForeignKey("GroupId")]
    public virtual Group Group { get; set; }
}

public class ScheduleLoad
{
    [Key]
    public int Id { get; set; }
    public DateTime LoadDate { get; set; }
    
    public virtual ICollection<Lesson> Lessons { get; set; }
}
