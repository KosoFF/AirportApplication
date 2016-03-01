using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Airport.Account;
using Airport.Core.Commands;
using Airport.Core.DependencyInjection;
using Airport.Core.Helpers;
using Airport.Core.MessageServices;
using Airport.SqlLinkService;

namespace Airport.ViewModels
{
    public class AddFlightViewModel : ViewModel
    {
        private IAccountService _accountService;
        #region Ctor

        public AddFlightViewModel()
        {

            AddFlightCommand = new RelayCommand(AddFlightCommandExecute, () => !IsLoading);
            BackCommand = new RelayCommand(NavigationService.GoBack, () => !IsLoading);
            _accountService = ServiceLocator.Locator.Get<IAccountService>();
            WelcomeText = "Welcome, " + _accountService.Account.FirstName;
            SetDictionaries();
#if DEBUG

#endif
        }


        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {

            AddFlightCommand.RaiseCanExecuteChanged();
            BackCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Fields
        private KeyValuePair<string, string> _aircraft;
        private KeyValuePair<string, string> _route;
        private Dictionary<string, string> _aircraftDictionary;
        private Dictionary<string, string> _routeDictionary; 
        private string _welcomeText;

        #endregion

        #region Properties

        
        public string WelcomeText
        {
            get { return _welcomeText; }
            set { Set(ref _welcomeText, value); }
        }

        public RelayCommand AddFlightCommand { get; set; }
        public RelayCommand BackCommand { get; set; }

        public Dictionary<string, string> AircraftDictionary
        {
            get { return _aircraftDictionary; }
            set { Set(ref _aircraftDictionary, value); }
        }

        public Dictionary<string, string> RouteDictionary
        {
            get { return _routeDictionary; }
            set { Set(ref _routeDictionary, value); }
        }

        public KeyValuePair<string, string> Aircraft
        {
            get { return _aircraft; }
            set { Set(ref _aircraft, value); }
        }

        public KeyValuePair<string, string> Route
        {
            get { return _route; }
            set { Set(ref _route, value); }
        }

        #endregion

        #region Private methods

        private async void AddFlightCommandExecute()
        {
            var sql = ServiceLocator.Locator.Get<ISqlLinkService>();
            var messageService = ServiceLocator.Locator.Get<IMessageService>();

            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }
            if (string.IsNullOrWhiteSpace(Aircraft.Key) || (string.IsNullOrWhiteSpace(Route.Key)))
            {
                await messageService.ShowAsync("Error", "Select departure and arrival points.");
                return;
            }
            IsLoading = true;
            if (await sql.AddFlightAsync(Route.Key, Aircraft.Key))
            {
                await messageService.ShowAsync("Success", "Flight was successfully added");
            }
            else
            {
                await
                    messageService.ShowAsync("Error",
                        "This flight already exist or there are some troubles with database");
            }
            IsLoading = false;
        }
   
        private async void SetDictionaries()
        {
            var sql = ServiceLocator.Locator.Get<ISqlLinkService>();
            IsLoading = true;
            AircraftDictionary = await sql.GetAircraftDictionaryAsync();
            RouteDictionary = await sql.GetRouteDictionaryAsync();
            IsLoading = false;
        }
        #endregion
    }
}
