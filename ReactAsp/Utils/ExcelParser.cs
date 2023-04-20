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
                var excelRange = _worksheet.Cells[row, col];
                var temp = excelRange.Text;

                if (string.IsNullOrEmpty(temp)) continue;

                var isOdd = Convert.ToInt16(excelRange.Address[^1]) % 2 == 0;
                var week = isOdd ? "1" : "2";

                var dayRow = row / 14 * 14 + 4;
                var partDay = _worksheet.Cells[dayRow, 1];

                var timeRow = row % 2 == 0 ? row : row - 1;
                var partTime = _worksheet.Cells[timeRow, 2];

                var times = partTime.Text.Split(" - ");
                var startTime = times[0];
                var endTime = times[1];
                
                var value = string.Join("#", excelRange.Text.Split("\n"));
                
                str.Append($"{partDay.Text}#{startTime}#{endTime}#{value}#{week}");
                teacherSchedule.Lessons.Add(str.ToString());
                str.Clear();
            }

            teacherSchedules.Add(teacherSchedule);
        }

        return teacherSchedules;
    }
}