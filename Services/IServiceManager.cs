using System;
using SM.Model;
using YASM.Model;

namespace YASM.Services
{
	public interface IServiceManager
	{
		IObservable<NotifyService> GetServices();
	}
}
