using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Prism.Mvvm;
using YASM.Model;
using YASM.Services;
using YASM.Utils;
using System.Threading.Tasks;

namespace YASM.ViewModels
{
    public class MainWindowViewModel : BindableBase
	{
		readonly IServiceManager _serviceManager;
		
		public ObservableCollection<NotifyService> Services { get; } = new ObservableCollection<NotifyService>();

        public IAsyncCommand StopServiceCommand { get; }

        public IAsyncCommand LoadServicesCommand { get; }

        NotifyService _selectedService;
		public NotifyService SelectedService 
		{ 
			get{ return _selectedService; }
			set{ SetProperty(ref _selectedService, value); }
		}
		
		bool _isLoading = false;
		public bool IsLoading
		{
			get{ return _isLoading; }
			set{ SetProperty(ref _isLoading, value); }
		}
		
		public MainWindowViewModel(IServiceManager serviceManager)
		{
			this._serviceManager = serviceManager;

            LoadServicesCommand = new AsyncCommand(s => LoadServicesAsync());
            LoadServicesCommand.Execute(null);

            StopServiceCommand = new AsyncCommand(s => StopServiceCommandAsync(), CanExecuteStop);
        }

        Task<object> LoadServicesAsync()
		{
            TaskCompletionSource<object> finalTask = new TaskCompletionSource<object>();

            _serviceManager.GetServices().ObserveOnDispatcher()
                .Subscribe(
                    Services.Add,
                    exception => finalTask.TrySetException(exception),
                    () => finalTask.SetResult(null) 
                );

            return finalTask.Task;
        }

        Task StopServiceCommandAsync()
        {
            return SelectedService.Stop();            
        }
		
		bool CanExecuteStop(object o)
		{
			return SelectedService != null && SelectedService.Status != System.ServiceProcess.ServiceControllerStatus.Stopped;
		}
	}
}
