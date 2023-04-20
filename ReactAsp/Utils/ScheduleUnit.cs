namespace ReactAsp.Utils;

public class ScheduleUnit
{
    public List<TeacherSchedule> Schedule { get; }
    public HashSet<string> Classrooms { get; }
    public HashSet<string> Groups { get; }
    public HashSet<string> Subjects { get; }
}