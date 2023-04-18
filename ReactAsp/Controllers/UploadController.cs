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
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;
            var colCount = worksheet.Dimension.Columns;

            for (int row = 1; row <= rowCount; row++)
            {
                for (int col = 1; col <= colCount; col++)
                {
                    var cellValue = worksheet.Cells[row, col].Value;
                    Console.Write("{0}\t", cellValue != null ? cellValue.ToString() : "");
                }
                Console.WriteLine();
            }
        }


        return Ok(new { message = "File uploaded successfully" });

    }
}