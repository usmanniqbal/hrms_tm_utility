using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_TM_Utility
{
	public class Profile
	{
		public Guid Id { get; set; }
		public string ProfileName { get; set; }
		public string StoreName { get; set; }
		public string Folder { get; set; }
		public string ArchiveFolder { get; set; }
	}
}
