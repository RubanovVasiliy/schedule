using System.Text;
using OfficeOpenXml;

namespace ReactAsp.Utils;

public class ExcelParser
{
    private int _columnStartIndex = 3;
    private int _rowStartIndex = 4;
    
    private readonly ExcelWorksheet _worksheet;

    public ExcelParser(ExcelWorksheet worksheet)
    {
        _worksheet = worksheet;
    }

    public ScheduleUnit ParseData()
    {
        var unit = new ScheduleUnit();

        for (var col = _columnStartIndex; col <= _worksheet.Dimension.End.Column; col++)
        {
            var teacherSchedule = new TeacherSchedule(_worksheet.Cells[2, col].Text);
            var str = new StringBuilder();

            for (var row = _rowStartIndex; row <= _worksheet.Dimension.End.Row; row++)
            {
                if (string.IsNullOrEmpty(_worksheet.Cells[row, col].Text)) continue;

                var week = GetWeek(row, col);
                var dayOfWeek = GetDayOfWeek(row);
                var (startTime, endTime) = GetTime(row);

                var subjectItems = _worksheet.Cells[row, col].Text.Split("\n");

                AddUniqueItems(unit, subjectItems);

                var subject = string.Join("#", subjectItems);

                str.Append($"{dayOfWeek}#{startTime}#{endTime}#{subject}#{week}");
                teacherSchedule.Lessons.Add(str.ToString());
                str.Clear();
            }

            unit.Schedule.Add(teacherSchedule);
        }

        return unit;
    }

    private string GetDayOfWeek(int row)
    {
        var dayRow = row / 14 * 14 + _rowStartIndex + 1;
        var partDay = _worksheet.Cells[dayRow, 1].Text;
        return partDay;
    }

    private KeyValuePair<string, string> GetTime(int row)
    {
        var timeRow = row % 2 == 1 ? row : row - 1;
        var partTime = _worksheet.Cells[timeRow, 2];
        var times = partTime.Text.Split(" - ");

        return new KeyValuePair<string, string>(times[0], times[1]);
    }

    private string GetWeek(int row, int col)
    {
        if (_worksheet.Cells[row, col].Merge) return "3";
        
        var isOdd = Convert.ToInt16(_worksheet.Cells[row, col].Address[^1]) % 2 == 1;
        var week = isOdd ? "1" : "0";
        return week;
    }

    private static void AddUniqueItems(ScheduleUnit unit, IReadOnlyList<string> items)
    {
        unit.Subjects.Add(items[0]);
        switch (items.Count)
        {
            case 3:
                unit.Classrooms.Add(items[2]);
                break;
            case 2:
            {
                foreach (var i in new List<string>(items[1].Split(", "))) unit.Groups.Add(i);
                break;
            }
        }
    }
}