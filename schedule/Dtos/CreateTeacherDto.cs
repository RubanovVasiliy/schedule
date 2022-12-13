using System.ComponentModel.DataAnnotations;

namespace schedule.Dtos;

public class CreateTeacherDto
{
    [Required] public string Fullname { get; set; }
}