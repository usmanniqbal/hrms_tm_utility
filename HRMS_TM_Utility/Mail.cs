using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_TM_Utility
{
	public class Mail
	{
		public string Email { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public TimeSpan TimeIn { get; set; }
		public TimeSpan TimeOut { get; set; }
		public decimal Hours { get; set; }
		public bool Status { get; set; }
		public string Remarks { get; set; }
		public DateTime ReceivedTime { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
	}
}
