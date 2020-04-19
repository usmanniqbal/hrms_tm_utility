using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
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
			var excelworkBook = excel.Workbooks.Add();
			var excelSheet = (Worksheet)excelworkBook.ActiveSheet;
			excelSheet.Name = "Time sheet";

			ExcelEntity e;
			int rowIndex = 1;
			excelSheet.Cells[rowIndex, 1] = nameof(e.Name);
			excelSheet.Cells[rowIndex, 2] = nameof(e.Email);
			excelSheet.Cells[rowIndex, 3] = nameof(e.Date);
			excelSheet.Cells[rowIndex, 4] = nameof(e.TimeIn);
			excelSheet.Cells[rowIndex, 5] = nameof(e.TimeOut);
			excelSheet.Cells[rowIndex, 6] = nameof(e.Hours);

			foreach (ExcelEntity entity in excelEntities)
			{
				rowIndex++;
				excelSheet.Cells[rowIndex, 1] = entity.Name;
				excelSheet.Cells[rowIndex, 2] = entity.Email;
				excelSheet.Cells[rowIndex, 3] = entity.Date;
				excelSheet.Cells[rowIndex, 4] = entity.TimeIn;
				excelSheet.Cells[rowIndex, 5] = entity.TimeOut;
				excelSheet.Cells[rowIndex, 6] = entity.Hours;
			}
			excelworkBook.SaveAs(path);
			excelworkBook.Close();
		}
	}

	public interface IExcelService
	{
		void Export(string path, List<ExcelEntity> excelEntities);
	}
}
