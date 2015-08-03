using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;

namespace YASM
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			
			var bs = new Bootstrapper();
			bs.Run();
		}
	}
}