using System.ComponentModel.DataAnnotations;

namespace schedule.Models;

public class Semester
{
    [Key] public Guid Id { get; set; }
    [Required] public string Title { get; set; }
}
