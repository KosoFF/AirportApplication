using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization.DateTimeFormatting;
using Airport.Account;
using Airport.Core.Commands;
using Airport.Core.DependencyInjection;
using Airport.Core.Helpers;
using Airport.Core.MessageServices;
using Airport.SqlLinkService;

namespace Airport.ViewModels
{
    public class AddAircraftViewModel : ViewModel
    {
        #region Ctor

        public AddAircraftViewModel()
        {
            AddAircraftCommand = new RelayCommand(AddAircraftCommandExecute, () => !IsLoading);
            BackCommand = new RelayCommand(NavigationService.GoBack, ()=>!IsLoading);
            var accountService = ServiceLocator.Locator.Get<IAccountService>();
            WelcomeText = "Welcome, " + accountService.Account.FirstName;
            BuiltDate = DateTime.Now;
#if DEBUG

#endif
        }


        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {

            AddAircraftCommand.RaiseCanExecuteChanged();
            BackCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Fields


        private string _welcomeText;
        private DateTimeOffset _builtDate;
        private string _seatsNum;
        private string _aircraftName;
        
        #endregion

        #region Properties

        public RelayCommand AddAircraftCommand { get; set; }
        public RelayCommand BackCommand { get; set; }


        public string WelcomeText
        {
            get { return _welcomeText; }
            set { Set(ref _welcomeText, value); }
        }

        public DateTimeOffset BuiltDate
        {
            get { return _builtDate; }
            set { Set(ref _builtDate, value); }
        }

        public string AircraftName
        {
            get { return _aircraftName; }
            set { Set(ref _aircraftName, value); }
        }

        public string SeatsNum
        {
            get { return _seatsNum; }
            set { Set(ref _seatsNum, value); }
        }

        #endregion

        #region Private methods

        private async void AddAircraftCommandExecute()
        {
            var sql = ServiceLocator.Locator.Get<ISqlLinkService>();
            var messageService = ServiceLocator.Locator.Get<IMessageService>();
            int seats;
            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }

            if (!int.TryParse(SeatsNum, out seats))
            {
                await messageService.ShowAsync("Error", "Enter correct seats number");
            }
            if (string.IsNullOrWhiteSpace(AircraftName))
            {
                await messageService.ShowAsync("Error", "Enter aircraft name.");
                return;
            }
            if (await sql.AddAircraftAsync(AircraftName, BuiltDate.DateTime, seats))
            {
                await messageService.ShowAsync("Success", "Aircraft was successfully added.");
                NavigationService.GoBack();
            }
            else
            {
                await messageService.ShowAsync("Error", "Something went wrong");
            }
        }
        #endregion
    }
}
