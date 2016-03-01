using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Airport.Account;
using Airport.Core.Commands;
using Airport.Core.DependencyInjection;
using Airport.Core.Helpers;
using Airport.Core.MessageServices;

namespace Airport.ViewModels
{
    public class ControlViewModel : ViewModel
    {

        
        #region Ctor

        public ControlViewModel()
        {

            AddFlightCommand = new RelayCommand(()=>NavigationService.Navigate(ViewLocator.AddFlight), () => !IsLoading);
            // BackCommand = new RelayCommand(() => NavigationService.GoBack());
            AddAircraftCommand = new RelayCommand(() => NavigationService.Navigate(ViewLocator.AddAircraft), () => !IsLoading);
            AddRouteCommand = new RelayCommand(() => NavigationService.Navigate(ViewLocator.AddRoute), () => !IsLoading);
            RegisterPassengerCommand = new RelayCommand(() => NavigationService.Navigate(ViewLocator.PassengerRegister), () => !IsLoading);
            CheckInCommand = new RelayCommand(() => NavigationService.Navigate(ViewLocator.CheckIn), () => !IsLoading);
            LogoutCommand = new RelayCommand(LogoutCommandExecute, () => !IsLoading);
            accountService = ServiceLocator.Locator.Get<IAccountService>();
            Username = "Welcome, " + accountService.Account.FirstName;
#if DEBUG

#endif
        }

       


        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            AddAircraftCommand?.RaiseCanExecuteChanged();
            AddFlightCommand?.RaiseCanExecuteChanged();
            AddRouteCommand?.RaiseCanExecuteChanged();
            RegisterPassengerCommand?.RaiseCanExecuteChanged();
            CheckInCommand?.RaiseCanExecuteChanged();
            LogoutCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Fields

        private IAccountService accountService;
        private string _username;
       

        #endregion

        #region Properties

        public string Username
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }

        public RelayCommand CheckInCommand { get; set; }

        public RelayCommand RegisterPassengerCommand { get; set; }

        public RelayCommand AddAircraftCommand { get; set; }

        public RelayCommand AddRouteCommand { get; set; }

        public RelayCommand AddFlightCommand { get; set; }

        public RelayCommand LogoutCommand { get; set; }

        #endregion

        #region Private methods
        
        private void LogoutCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.Login);
        }
       

        #endregion
    }
}
