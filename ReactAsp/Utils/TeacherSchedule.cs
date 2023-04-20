namespace ReactAsp.Utils;

public class TeacherSchedule
{
    public string TeacherName { get; }
    public List<string> Lessons { get; } = new();

    public TeacherSchedule(string teacherName)
    {
        TeacherName = teacherName;
    }
}