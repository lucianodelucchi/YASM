﻿using System.ServiceProcess;
using System.Threading.Tasks;
using Prism.Mvvm;
using SM.Model;
using YASM.Utils;

namespace YASM.Model
{
    public class NotifyService : BindableBase
	{
		IService _service;
		
		NotifyService(IService service)
		{
			_service = service;
		}
		
		public static NotifyService FromService(IService service)
		{
			return new NotifyService(service);
		}

		public string ServiceName 
		{
			get 
			{
				return _service.ServiceName;
			}
		}

		public string Description 
		{
			get 
			{
				return _service.Description;
			}
		}

		public string StartName 
		{
			get 
			{
				return _service.StartName;
			}
		}

		public string DisplayName 
		{
			get 
			{
				return _service.DisplayName;
			}
		}

        public ServiceControllerStatus Status
        {
            get
            {
                return _service.Status;
            }
        }

        public NotifyTask<ServiceControllerStatus> StatusAsync 
		{
			get 
			{
				return NotifyTask.Create<ServiceControllerStatus>(GetStatusAsync());
			}
		}		
		
		async Task<ServiceControllerStatus> GetStatusAsync()
		{
			await _service.Refresh();
			return await _service.GetStatus();
		}

		Task ChangeStatusTo(ServiceControllerStatus status)
		{
			Task t;
			switch (status) 
			{
				case ServiceControllerStatus.Running:
					t = _service.Start();
					break;
				case ServiceControllerStatus.Stopped:
					t = _service.Stop();
					break;
				case ServiceControllerStatus.Paused:
					t = _service.Pause();
					break;
				default:
					t = Task.Factory.StartNew(() => {});
					break;
			}
			
			OnPropertyChanged(() => StatusAsync);
			return t;
		}
		
		public Task Stop()
		{
			return ChangeStatusTo(ServiceControllerStatus.Stopped);
		}
	}
}
