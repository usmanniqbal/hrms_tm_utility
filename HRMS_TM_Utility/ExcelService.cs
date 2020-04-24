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
				workSheet.Cells[rowIndex, ExcelColumns.Name] = nameof(e.Name);
				workSheet.Cells[rowIndex, ExcelColumns.Email] = nameof(e.Email);
				workSheet.Cells[rowIndex, ExcelColumns.EmployeeCode] = nameof(e.EmployeeCode);
				workSheet.Cells[rowIndex, ExcelColumns.Date] = nameof(e.Date);
				workSheet.Cells[rowIndex, ExcelColumns.TimeIn] = nameof(e.TimeIn);
				workSheet.Cells[rowIndex, ExcelColumns.TimeOut] = nameof(e.TimeOut);
				workSheet.Cells[rowIndex, ExcelColumns.Hours] = nameof(e.Hours);
				workSheet.Cells[rowIndex, ExcelColumns.Remarks] = nameof(e.Remarks);

				foreach (ExcelEntity entity in excelEntities)
				{
					rowIndex++;
					workSheet.Cells[rowIndex, ExcelColumns.Name] = entity.Name;
					workSheet.Cells[rowIndex, ExcelColumns.Email] = entity.Email;
					workSheet.Cells[rowIndex, ExcelColumns.EmployeeCode] = entity.EmployeeCode;
					workSheet.Cells[rowIndex, ExcelColumns.Date] = entity.Date;
					workSheet.Cells[rowIndex, ExcelColumns.TimeIn] = entity.TimeIn;
					workSheet.Cells[rowIndex, ExcelColumns.TimeOut] = entity.TimeOut;
					workSheet.Cells[rowIndex, ExcelColumns.Hours] = entity.Hours;
					workSheet.Cells[rowIndex, ExcelColumns.Remarks] = entity.Remarks;
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

	public enum ExcelColumns : int
	{
		EmployeeCode = 1,
		Name,
		Email,
		Date,
		TimeIn,
		TimeOut,
		Hours,
		Remarks
	}
}
