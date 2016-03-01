using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airport.Account;
using Airport.Core.Commands;
using Airport.Core.DependencyInjection;
using Airport.Core.Helpers;
using Airport.Core.MessageServices;
using Airport.SqlLinkService;

namespace Airport.ViewModels
{
    public class AddRouteViewModel : ViewModel
    {
        #region Ctor

        public AddRouteViewModel()
        {
            AddRouteCommand = new RelayCommand(AddRouteCommandExecute, () => !IsLoading);
            AddCityCommand = new RelayCommand(AddCityCommandExecute, ()=> !IsLoading);
            BackCommand = new RelayCommand(()=>NavigationService.GoBack(), ()=> !IsLoading);
            var accountService = ServiceLocator.Locator.Get<IAccountService>();
            WelcomeText = "Welcome, " + accountService.Account.FirstName;
            SetCitiesList();
#if DEBUG

#endif
        }
        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
         
            AddRouteCommand.RaiseCanExecuteChanged();
            AddCityCommand.RaiseCanExecuteChanged();
            BackCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Fields

        private Dictionary<string, string> _citiesDictionary; 
        private string _welcomeText;
        private string _cityName;
        private KeyValuePair<string,string> _departurePoint;
        private KeyValuePair<string, string> _arrivalPoint;
        #endregion

        #region Properties

      
        public string WelcomeText
        {
            get { return _welcomeText; }
            set { Set(ref _welcomeText, value); }
        }

        public RelayCommand AddRouteCommand { get; set; }
        public RelayCommand AddCityCommand { get; set; }
        public RelayCommand BackCommand { get; set; }

        public Dictionary<string, string> CitiesDictionary
        {
            get { return _citiesDictionary; }
            set { Set(ref _citiesDictionary, value); }
        }

        public string CityName
        {
            get { return _cityName; }
            set { Set(ref _cityName, value); }
        }

        public KeyValuePair<string, string> DeparturePoint
        {
            get { return _departurePoint; }
            set { Set(ref _departurePoint, value); }
        }

        public KeyValuePair<string, string> ArrivalPoint
        {
            get { return _arrivalPoint; }
            set { Set(ref _arrivalPoint, value);  }
        }

        #endregion

        #region Private methods
        private async void AddCityCommandExecute()
        {
            var sql = ServiceLocator.Locator.Get<ISqlLinkService>();
            var messageService = ServiceLocator.Locator.Get<IMessageService>();

            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }
            if (string.IsNullOrWhiteSpace(CityName))
            {
                await messageService.ShowAsync("Error", "Enter city name");
                return;
            }
            IsLoading = true;
            if (await sql.AddCityAsync(CityName))
            {
                await messageService.ShowAsync("Success", "City was successfully added.");
                SetCitiesList();
            }
            else
            {
                await messageService.ShowAsync("Error", "This city already exists or something went wrong.");
            }
            IsLoading = false;

        }

        private async void SetCitiesList()
        {
            var sql = ServiceLocator.Locator.Get<ISqlLinkService>();
            IsLoading = true;
            CitiesDictionary = await sql.GetCityDictionaryAsync();
            IsLoading = false;
        }
        private async void AddRouteCommandExecute()
        {

            var sql = ServiceLocator.Locator.Get<ISqlLinkService>();
            var messageService = ServiceLocator.Locator.Get<IMessageService>();

            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ArrivalPoint.Key) || (string.IsNullOrWhiteSpace(DeparturePoint.Key)))
            {
                await messageService.ShowAsync("Error", "Select departure and arrival points.");
                return;
            }
            else if (string.Equals(ArrivalPoint.Key, DeparturePoint.Key))
            {
                await messageService.ShowAsync("Error", "You shall choose different arrival and departure points.");
                return;
            }
            IsLoading = true;
            if (await sql.AddRouteAsync(DeparturePoint.Key, ArrivalPoint.Key))
            {
                await messageService.ShowAsync("Success", "Route was successfully added.");
                NavigationService.GoBack();
            }
            else
            {
                await messageService.ShowAsync("Error", "Could not add route. Check if it is existing");
            }
            IsLoading = false;

        }
        #endregion
    }
}
