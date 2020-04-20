using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_TM_Utility
{
	public class ExcelService : IExcelService
	{
		public void Export(string path, List<ExcelEntity> excelEntities)
		{
			var excel = new Application();
			Workbook workBook = null;
			Worksheet workSheet = null;

			try
			{
				if (File.Exists(path))
				{
					workBook = excel.Workbooks.Open(path, ReadOnly: false);

					foreach (Worksheet ws in workBook.Sheets)
					{
						if (ws.Name == "WFH")
						{
							workSheet = ws;
							break;
						}
					}

					if(workSheet != null)
					{
						workSheet.Cells.Clear();
					}
					else
					{
						workSheet = (Worksheet)workBook.Worksheets.Add();
						workSheet.Name = "WFH";
					}
				}
				else
				{
					workBook = excel.Workbooks.Add();
					workSheet = (Worksheet)workBook.ActiveSheet;
					workSheet.Name = "WFH";
				}

				ExcelEntity e;
				int rowIndex = 1;
				workSheet.Cells[rowIndex, 1] = nameof(e.Name);
				workSheet.Cells[rowIndex, 2] = nameof(e.Email);
				workSheet.Cells[rowIndex, 3] = nameof(e.Date);
				workSheet.Cells[rowIndex, 4] = nameof(e.TimeIn);
				workSheet.Cells[rowIndex, 5] = nameof(e.TimeOut);
				workSheet.Cells[rowIndex, 6] = nameof(e.Hours);

				foreach (ExcelEntity entity in excelEntities)
				{
					rowIndex++;
					workSheet.Cells[rowIndex, 1] = entity.Name;
					workSheet.Cells[rowIndex, 2] = entity.Email;
					workSheet.Cells[rowIndex, 3] = entity.Date;
					workSheet.Cells[rowIndex, 4] = entity.TimeIn;
					workSheet.Cells[rowIndex, 5] = entity.TimeOut;
					workSheet.Cells[rowIndex, 6] = entity.Hours;
				}
				workBook.SaveAs(path);
			}
			finally
			{
				workBook?.Close();
			}
		}
	}

	public interface IExcelService
	{
		void Export(string path, List<ExcelEntity> excelEntities);
	}
}
