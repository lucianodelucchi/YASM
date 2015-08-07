using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using SM.Model;
using YASM.Model;
using YASM.Services;

namespace YASM.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		readonly IServiceManager _serviceManager;
		
		public ObservableCollection<NotifyService> Services { get; private set; }
		
		public ICommand TestCommand { get; set; }
		
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
			TestCommand = new DelegateCommand(Execute, CanExecute).ObservesProperty(() => SelectedService);
			
			Services = new ObservableCollection<NotifyService>();
			
			this._serviceManager = serviceManager;
			
			this.LoadServices();
		}
		
		
		void LoadServices()
		{
			IsLoading = true;
			_serviceManager.GetServices().ObserveOnDispatcher()
				.Subscribe(
					Services.Add,
					()=> IsLoading = false
			);
		}
		
		void Execute()
		{
			SelectedService.Stop();
		}
		
		bool CanExecute()
		{
			return SelectedService != null;
		}
	}
}
