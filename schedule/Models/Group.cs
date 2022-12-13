using System.ComponentModel.DataAnnotations;

namespace schedule.Models;

public class Group
{
    [Key] public Guid Id { get; set; }
    [Required] public string Title { get; set; }
}