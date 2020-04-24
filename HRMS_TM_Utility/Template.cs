using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_TM_Utility
{

	public class MainTemplate
	{
		public WFHTemplate wfh { get; set; }
	}

	public class WFHTemplate
	{
		public SearchTemplate[] date { get; set; }
		public SearchTemplate[] empcode { get; set; }
		public SearchTemplate[] timein { get; set; }
		public SearchTemplate[] timeout { get; set; }
		public SearchTemplate[] hours { get; set; }
	}

	public class SearchTemplate
	{
		public string search { get; set; }
		public string extract { get; set; }
	}
}
