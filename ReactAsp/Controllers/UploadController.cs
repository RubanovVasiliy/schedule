using System.Text;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace ReactAsp.Controllers;


[ApiController]
[Route("[controller]")]
public class UploadController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Upload()
    {
        var file = Request.Form.Files[0];
        var filePath = Path.GetTempFileName();

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets["TDSheet"];
            var str = new StringBuilder();
            for (var col = 3; col <= worksheet.Dimension.End.Column; col++)
            {
                Console.Write(worksheet.Cells[2, col].Text + "\n");
                for (var row = 3; row <= worksheet.Dimension.End.Row; row++)
                {
                    var excelRange = worksheet.Cells[row, col];

                    var temp = excelRange.Text;
                    if (string.IsNullOrEmpty(temp)) continue;
                    var partDay = worksheet.Cells[row, 1];
                    var partTime = worksheet.Cells[row, 2];
                    var value = string.Join(" ", excelRange.Text.Split("\n"));
                    str.Append($"{partDay.Text} - {partTime.Text} {value}");
                    str.Append(Convert.ToInt16(excelRange.Address[^1]) % 2 == 1 ? " 2" : " 1");
                    Console.WriteLine(str);
                }
                Console.WriteLine();
            }
        }


        return Ok(new { message = "File uploaded successfully" });

    }
}