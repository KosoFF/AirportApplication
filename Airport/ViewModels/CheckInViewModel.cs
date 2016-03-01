using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Airport.Account;
using Airport.Core.Commands;
using Airport.Core.DependencyInjection;
using Airport.Core.Helpers;
using Airport.Core.MessageServices;
using Airport.SqlLinkService;
using Airport.SqlReference;

namespace Airport.ViewModels
{
    public class CheckInViewModel : ViewModel
    {
        private IAccountService _accountService;
        #region Ctor

        public CheckInViewModel()
        {

            AddBoardingPassCommand = new RelayCommand(AddBoardingPassCommandExecute, () => !IsLoading);
            BackCommand = new RelayCommand(NavigationService.GoBack, () => !IsLoading);
            SearchCommand = new RelayCommand(OnSearchCommandExecute, ()=>!IsLoading);
            _accountService = ServiceLocator.Locator.Get<IAccountService>();
            FlightDateTime = DateTimeOffset.Now.Date;
            FlightTime = DateTimeOffset.Now.TimeOfDay;
            SetDictionaries();
#if DEBUG

#endif
        }

       
        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {

            AddBoardingPassCommand.RaiseCanExecuteChanged();
            BackCommand.RaiseCanExecuteChanged();
            SearchCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Fields
        
        private Dictionary<string, string> _passengerDictionary;
        private Dictionary<string, string> _flightDictionary;
        private Dictionary<string, string> _cityDictionary;
        private Dictionary<string, string> _passengerBufferDictionary;
        private string _city;
        private string _passenger;
        private string _flight;
        private string _destination;
        private string _weight;
        private string _flightSearchValue;
        private bool _hasLuggage;
        private DateTimeOffset _flightDateTime;
        private TimeSpan _flightTime;
        

        #endregion

        #region Properties



        public RelayCommand SearchCommand { get; set; }
        public RelayCommand AddBoardingPassCommand { get; set; }
        public RelayCommand BackCommand { get; set; }

        public Dictionary<string, string> PassengerDictionary
        {
            get { return _passengerDictionary; }
            set { Set(ref _passengerDictionary, value); }
        }

        public Dictionary<string, string> FlightDictionary
        {
            get { return _flightDictionary; }
            set { Set(ref _flightDictionary, value); }
        }

        public string Destination
        {
            get { return _destination; }
            set { Set(ref _destination, value); }
        }

        public Dictionary<string, string> CityDictionary
        {
            get { return _cityDictionary; }
            set { Set(ref _cityDictionary, value); }
        }

        public string City
        {
            get { return _city; }
            set
            {
                Set(ref _city, value); 
                OnCitySelectionChanged();
            }
        }

        public string Passenger
        {
            get { return _passenger; }
            set { Set(ref _passenger, value); }
        }

        public string Flight
        {
            get { return _flight; }
            set { Set(ref _flight, value); }
        }

        public bool HasLuggage
        {
            get { return _hasLuggage; }
            set { Set(ref _hasLuggage, value); }
        }

        public DateTimeOffset FlightDateTime
        {
            get { return _flightDateTime; }
            set { Set(ref _flightDateTime, value); }
        }

        public TimeSpan FlightTime
        {
            get { return _flightTime; }
            set { Set(ref _flightTime, value);
                FlightDateTime=FlightDateTime.Date.Add(FlightTime);
            }
        }

        public string Weight
        {
            get { return _weight; }
            set { Set(ref _weight, value); }
        }

        public Dictionary<string, string> PassengerBufferDictionary
        {
            get { return _passengerBufferDictionary; }
            set { Set(ref _passengerBufferDictionary, value); }
        }

        public string FlightSearchValue
        {
            get { return _flightSearchValue; }
            set { Set(ref _flightSearchValue, value); }
        }

        #endregion

        #region Private methods

        private async void AddBoardingPassCommandExecute()
        {
            var sql = ServiceLocator.Locator.Get<ISqlLinkService>();
            var messageService = ServiceLocator.Locator.Get<IMessageService>();

            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }
            if (Flight == null || Passenger == null)
            {
                await messageService.ShowAsync("Error", "Fill all the fields.");
                return;
            }
            float floatWeight = new float();
            if (HasLuggage && !float.TryParse(Weight, out floatWeight))
            {
                await messageService.ShowAsync("Error", "Enter correct weight.");
                return;
            }
            IsLoading = true;

            var response = await sql.AddBoardingPass(Flight, Passenger, _accountService.Account.UserId,
                FlightDateTime.DateTime);
            if (response == null)
            {
                await messageService.ShowAsync("Error", "Failed to add boarding pass.");
                IsLoading = false;
                return;
            }
            if (HasLuggage)
            {
                var luggageResponse = await sql.AddLuggage(response, floatWeight);
                if (luggageResponse == null)
                {
                    await messageService.ShowAsync("Error", "Failed to add luggage.");
                    IsLoading = false;
                    return;
                }
            }
            await messageService.ShowAsync("Success", "Boarding pass was added.");
            
           

            //if (await sql.AddFlightAsync(Route.Key, Aircraft.Key))
            //{
            //    await messageService.ShowAsync("Success", "Flight was successfully added");
            //}
            //else
            //{
            //    await
            //        messageService.ShowAsync("Error",
            //            "This flight already exist or there are some troubles with database");
            //}
            IsLoading = false;
        }

        private async void SetDictionaries()
        {
            var sql = ServiceLocator.Locator.Get<ISqlLinkService>();
            IsLoading = true;
            PassengerDictionary = 
            PassengerBufferDictionary = await sql.GetPassengerDictionaryAsync();
            PassengerDictionary = PassengerBufferDictionary;
            FlightDictionary = await sql.GetFlightDictionaryAsync();
            CityDictionary = await sql.GetCityDictionaryAsync();
            IsLoading = false;
        }
        private async void OnCitySelectionChanged()
        {
            var sql = ServiceLocator.Locator.Get<ISqlLinkService>();
            IsLoading = true;
            FlightDictionary = await sql.GetFlightDictionaryAsync(null, City);
            IsLoading = false;
        }
        private void OnSearchCommandExecute()
        {
            IsLoading = true;
            PassengerDictionary = PassengerBufferDictionary.Where(c=>c.Value.IndexOf(FlightSearchValue, StringComparison.OrdinalIgnoreCase) >= 0).ToDictionary(c=>c.Key,d=>d.Value);
            IsLoading = false;
        }

        #endregion
    }
}
