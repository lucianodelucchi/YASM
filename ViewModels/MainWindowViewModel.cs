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
		
		public IService SelectedService { get; set; }
		
		public MainWindowViewModel(IServiceManager serviceManager)
		{
			TestCommand = new DelegateCommand(Execute, CanExecute);
			
			Services = new ObservableCollection<IService>();
			
			this._serviceManager = serviceManager;
			this._serviceManager.GetServices().ObserveOnDispatcher().Subscribe(Services.Add);
		}
		
		void Execute()
		{
			var d = SelectedService.Description;
		}
		
		bool CanExecute()
		{
			return true;
		}
	}
}
