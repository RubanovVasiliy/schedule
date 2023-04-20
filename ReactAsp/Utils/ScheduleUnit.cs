namespace ReactAsp.Utils;

public class ScheduleUnit
{
    public List<TeacherSchedule> Schedule { get; } = new();
    public HashSet<string> Classrooms { get; } = new();
    public HashSet<string> Groups { get; } = new();
    public HashSet<string> Subjects { get; } = new();
}