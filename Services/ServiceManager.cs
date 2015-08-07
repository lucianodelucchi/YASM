using System;
using System.Reactive.Linq;
using SM.Model;
using YASM.Model;

namespace YASM.Services
{
	public class ServiceManager : IServiceManager
	{
		public ServiceManager()
		{
		}

		#region IServiceManager implementation

		public IObservable<NotifyService> GetServices()
		{
			return SM.ServiceManager.GetServices().Select(NotifyService.FromService);
		}

		#endregion
	}
}
