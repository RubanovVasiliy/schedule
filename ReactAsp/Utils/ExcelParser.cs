using System.Text;
using System.Text.RegularExpressions;
using OfficeOpenXml;

namespace ReactAsp.Utils;

public class ExcelParser
{
    private const int ColumnStartIndex = 3;
    private const int RowStartIndex = 4;

    private const string PatternGroup = @"\w{2}-?\d{3}";
    private const string PatternClass = @"\w\.\d{3}\w?\s?\(\w\.\d\)?";

    public ScheduleUnit Unit { get; } = new();

    private readonly ExcelWorksheet _worksheet;

    public ExcelParser(ExcelWorksheet worksheet)
    {
        _worksheet = worksheet;
    }

    public void ParseData()
    {
        var lessonString = new StringBuilder();
        var subjectString = new StringBuilder();

        for (var col = ColumnStartIndex; col <= _worksheet.Dimension.End.Column; col++)
        {
            var teacherSchedule = new TeacherSchedule(_worksheet.Cells[RowStartIndex - 1, col].Text);

            for (var row = RowStartIndex; row <= _worksheet.Dimension.End.Row; row++)
            {
                if (string.IsNullOrEmpty(_worksheet.Cells[row, col].Text)) continue;

                var week = GetWeek(row, col);
                var dayOfWeek = GetDayOfWeek(row);
                var (startTime, endTime) = GetTime(row);

                subjectString.Append(_worksheet.Cells[row, col].Text)
                    .Replace('\n', ' ')
                    .Replace("  ", " ");

                var subject = ParseSubjectGroupClassroomString(subjectString);

                var subjectGroupClassroom = string.Join("#", subject);

                lessonString.Append($"{dayOfWeek}#{startTime}#{endTime}#{subjectGroupClassroom}#{week}");
                teacherSchedule.Lessons.Add(lessonString.ToString());
                lessonString.Clear();
                subjectString.Clear();
            }

            Unit.Schedule.Add(teacherSchedule);
        }
    }

    private string GetDayOfWeek(int row)
    {
        var dayRow = (row - RowStartIndex) / 14 * 14 + RowStartIndex + 1;
        var partDay = _worksheet.Cells[dayRow, 1].Text;
        return partDay;
    }

    private KeyValuePair<string, string> GetTime(int row)
    {
        var timeRow = row % 2 == 1 ? row : row - 1;
        var partTime = _worksheet.Cells[timeRow, 2];
        var times = partTime.Text.Split(" - ");
        var timeStart = string.Join(':', times[0].Split(':').Take(2));
        var timeEnd = string.Join(':', times[1].Split(':').Take(2));

        return new KeyValuePair<string, string>(timeStart, timeEnd);
    }

    private string GetWeek(int row, int col)
    {
        if (_worksheet.Cells[row, col].Merge) return "3";

        var isOdd = Convert.ToInt16(_worksheet.Cells[row, col].Address[^1]) % 2 == 1;
        var week = isOdd ? "1" : "0";
        return week;
    }

    private IEnumerable<string> ParseSubjectGroupClassroomString(StringBuilder subjectString)
    {
        var newGroups = Regex.Matches(subjectString.ToString(), PatternGroup);
        var newClasses = Regex.Matches(subjectString.ToString(), PatternClass);

        var sItems = subjectString.ToString().Split(' ');

        var shift = sItems.Length - newGroups.Count - newClasses.Count * 2;
        if (sItems[^1].Equals("")) shift -= 1;

        var groups = new List<string>();
        var subject = new List<string> { string.Join(" ", sItems.Take(shift)) };

        Unit.AddSubject(subject[0]);

        foreach (Match group in newGroups)
        {
            var groupString = new StringBuilder(group.ToString());
            
            if (groupString.Length == 5)
            {
                groupString.Insert(2,'-');
            }

            groups.Add(groupString.ToString());
            Unit.AddGroup(groupString.ToString());
        }

        subject.Add(string.Join(",", groups));

        foreach (Match classroom in newClasses)
        {
            subject.Add(classroom.ToString());
            Unit.AddClassroom(classroom.ToString());
        }

        return subject;
    }
}