using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_TM_Utility
{
	public class ExcelEntity
	{
		public string Email { get; set; }
		public string Name { get; set; }
		public string EmployeeCode { get; set; }
		public DateTime Date { get; set; }
		public DateTime TimeIn { get; set; }
		public DateTime TimeOut { get; set; }
		public decimal Hours { get; set; }
		public string Remarks { get; set; }
	}
}
