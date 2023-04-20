// See https://aka.ms/new-console-template for more information


using OfficeOpenXml;
using ReactAsp.Utils;

class Program
{
    static void Main(string[] args)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var fileInfo = new FileInfo("../../../data/ex.xlsx");
        if (!fileInfo.Exists) return;

        using var package = new ExcelPackage(fileInfo);
        var worksheet = package.Workbook.Worksheets["TDSheet"];

        var parser = new ExcelParser(worksheet);
        var teacherSchedules = parser.ParseData();

        var set = new HashSet<string>();
        var list = new List<string>();

        foreach (var teacherSchedule in teacherSchedules)
        {
            foreach (var lesson in teacherSchedule.Lessons)
            {
                var subject = lesson.Split('#')[3];
                set.Add(subject);
                list.Add(subject);
            }
        }

        foreach (var i in set)
        {
            Console.WriteLine(i);
        }
        Console.WriteLine(set.Count);
    }
}