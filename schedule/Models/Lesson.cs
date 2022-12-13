using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace schedule.Models;

public class Lesson
{
    [Key] public Guid Id { get; set; }
    [Required] public string Title { get; set; }
    [Required] public string StartTime { get; set; }
    [Required] public string EndTime { get; set; }
    [Required] public string DayOfWeek { get; set; }
    [Required] public bool Even { get; set; }
    public List<Group> Groups { get; set; }
    [JsonIgnore] public Semester Semester { get; set; }
}