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
        try
        {
            if (Request.Form.Files.Count == 0)
            {
                return BadRequest(new { message = "File not found" });
            }
            
            var file = Request.Form.Files[0];
            
            if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return BadRequest(new { message = "Invalid file format. Please upload an Excel file" });
            }
            
            var filePath = Path.GetTempFileName();

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            var fileInfo = new FileInfo(filePath);
            //if (!fileInfo.Exists) return Ok(new { message = "File uploaded successfully" });

            using var package = new ExcelPackage(fileInfo);
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

                    var isOdd = Convert.ToInt16(excelRange.Address[^1]) % 2 == 0;

                    var dayRow = row / 14 * 14 + 4;
                    var partDay = worksheet.Cells[dayRow, 1];

                    var timeRow = row % 2 == 0 ? row : row - 1;
                    var partTime = worksheet.Cells[timeRow, 2];


                    var times = partTime.Text.Split(" - ");
                    var startTime = times[0];
                    var endTime = times[1];

                    var value = string.Join("#", excelRange.Text.Split("\n"));
                    str.Append(partDay.Text);
                    str.Append('#');
                    str.Append(startTime);
                    str.Append('#');
                    str.Append(endTime);
                    str.Append('#');
                    str.Append(value);
                    str.Append('#');
                    str.Append(isOdd ? "1" : "2");
                    Console.WriteLine(str);
                    str.Clear();
                }
 
                Console.WriteLine();
            }
            package.Dispose();
            
            return Ok(new { message = "File uploaded successfully" });
        }
        
        catch (Exception ex)
        {
            return BadRequest(new { message = $"Error: {ex.Message}" });
        }
    }
}