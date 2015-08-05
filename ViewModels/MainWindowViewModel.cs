using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using SM.Model;
using YASM.Services;

namespace YASM.ViewModels
{
	public class MainWindowViewModel : BindableBase
	{
		readonly IServiceManager _serviceManager;
		
		public ObservableCollection<IService> Services { get; private set; }
		
		public ICommand TestCommand { get; set; }
		
		IService _selectedService;
		public IService SelectedService 
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
			
			Services = new ObservableCollection<IService>();
			
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
		}
		
		bool CanExecute()
		{
			return SelectedService != null;
		}
	}
}
