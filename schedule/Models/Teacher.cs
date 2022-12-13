using System.ComponentModel.DataAnnotations;

namespace schedule.Models;

public class Teacher
{
    [Key] public Guid Id { get; set; }
    [Required] public string Fullname { get; set; }
    public List<Lesson> Lessons { get; set; }
}