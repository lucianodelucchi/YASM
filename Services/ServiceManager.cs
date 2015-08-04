using System;
using SM.Model;

namespace YASM.Services
{
	public class ServiceManager : IServiceManager
	{
		public ServiceManager()
		{
		}

		#region IServiceManager implementation

		public IObservable<IService> GetServices()
		{
			return SM.ServiceManager.GetServices();
		}

		#endregion
	}
}
