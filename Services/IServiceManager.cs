using System;
using SM.Model;

namespace YASM.Services
{
	public interface IServiceManager
	{
		IObservable<IService> GetServices();
	}
}
