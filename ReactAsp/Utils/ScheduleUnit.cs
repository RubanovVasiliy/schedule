namespace ReactAsp.Utils;

public class ScheduleUnit
{
    public List<TeacherSchedule> Schedule { get; } = new();
    public HashSet<string> Classrooms { get; } = new();
    public HashSet<string> Groups { get; } = new();
    public HashSet<string> Subjects { get; } = new();

    public void AddGroup(string group)
    {
        Groups.Add(group);
    }

    public void AddSubject(string subject)
    {
        Subjects.Add(subject);
    }

    public void AddClassroom(string classrooms)
    {
        Classrooms.Add(classrooms);
    }
}