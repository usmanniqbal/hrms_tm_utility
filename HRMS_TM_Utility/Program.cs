﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRMS_TM_Utility
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{

			Application.ThreadException += (sender, args) => MessageBox.Show(args.Exception.Message);
			AppDomain.CurrentDomain.UnhandledException += (sender, args) => MessageBox.Show(((Exception)args.ExceptionObject).Message);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmImport());
		}
	}
}
