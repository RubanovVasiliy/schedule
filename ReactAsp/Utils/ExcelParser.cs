using System.Text;
using OfficeOpenXml;

namespace ReactAsp.Utils;

public class ExcelParser
{
    private readonly ExcelWorksheet _worksheet;

    public ExcelParser(ExcelWorksheet worksheet)
    {
        _worksheet = worksheet;
    }

    public IEnumerable<TeacherSchedule> ParseData()
    {
        var teacherSchedules = new List<TeacherSchedule>();

        for (var col = 3; col <= _worksheet.Dimension.End.Column; col++)
        {
            var teacherSchedule = new TeacherSchedule(_worksheet.Cells[2, col].Text);
            var str = new StringBuilder();

            for (var row = 3; row <= _worksheet.Dimension.End.Row; row++)
            {
                if (string.IsNullOrEmpty(_worksheet.Cells[row, col].Text)) continue;

                var week = GetWeek(row, col);

                var dayOfWeek = GetDayOfWeek(row);

                var (startTime, endTime) = GetTime(row);

                var subject = string.Join("#", _worksheet.Cells[row, col].Text.Split("\n"));

                str.Append($"{dayOfWeek}#{startTime}#{endTime}#{subject}#{week}");
                teacherSchedule.Lessons.Add(str.ToString());
                str.Clear();
            }

            teacherSchedules.Add(teacherSchedule);
        }

        return teacherSchedules;
    }

    private string GetDayOfWeek(int row)
    {
        var dayRow = row / 14 * 14 + 4;
        var partDay = _worksheet.Cells[dayRow, 1].Text;
        return partDay;
    }

    private KeyValuePair<string, string> GetTime(int row)
    {
        var timeRow = row % 2 == 0 ? row : row - 1;
        var partTime = _worksheet.Cells[timeRow, 2];
        var times = partTime.Text.Split(" - ");

        return new KeyValuePair<string, string>(times[0], times[1]);
    }

    private string GetWeek(int row, int col)
    {
        var isOdd = Convert.ToInt16(_worksheet.Cells[row, col].Address[^1]) % 2 == 0;
        var week = isOdd ? "1" : "2";
        return week;
    }
}