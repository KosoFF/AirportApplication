using System;
using System.Collections.Generic;
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
    public class PassengerRegisterViewModel : ViewModel
    {
        #region Ctor

        public PassengerRegisterViewModel()
        {
            AddPassengerCommand = new RelayCommand(AddPassengerCommandExecute, () => !IsLoading);
            BackCommand = new RelayCommand(NavigationService.GoBack, () => !IsLoading);
            DeletePassengerCommand = new RelayCommand(DeletePassengerCommandExecute, () => !IsLoading);
            BirthDate= DateTimeOffset.Now;
            GetPassengerDictionary();
#if DEBUG

#endif
        }
       


        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {

            AddPassengerCommand.RaiseCanExecuteChanged();
            BackCommand.RaiseCanExecuteChanged();
            DeletePassengerCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Fields

        private bool _gender;
        private string _welcomeText;
        private DateTimeOffset _builtDate;
        private string _firstName;
        private string _lastName;
        private string _passportNum;
        private DateTimeOffset _birthDate;
        private Dictionary<string, string> _passengerDictionary;
        private string _selectedPassengerId;

        #endregion

        #region Properties

        public RelayCommand AddPassengerCommand { get; set; }
        public RelayCommand BackCommand { get; set; }
        public RelayCommand DeletePassengerCommand { get; set; }


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

        public bool Gender
        {
            get { return _gender; }
            set { Set(ref _gender, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { Set(ref _lastName, value); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { Set(ref _firstName, value); }
        }

        public string PassportNum
        {
            get { return _passportNum; }
            set { Set(ref _passportNum, value); }
        }

        public DateTimeOffset BirthDate
        {
            get { return _birthDate;}
            set { Set(ref _birthDate, value); }
        }

        public Dictionary<string, string> PassengerDictionary
        {
            get { return _passengerDictionary; }
            set { Set(ref _passengerDictionary, value);  }
        }

        public string SelectedPassengerId
        {
            get { return _selectedPassengerId; }
            set { Set(ref _selectedPassengerId, value); }
        }

        #endregion

        #region Private methods

        private async void AddPassengerCommandExecute()
        {
            var sql = ServiceLocator.Locator.Get<ISqlLinkService>();
            var messageService = ServiceLocator.Locator.Get<IMessageService>();
            long passportNum;
            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }
            IsLoading = true;
            //if (!int.TryParse(SeatsNum, out seats))
            //{
            //    await messageService.ShowAsync("Error", "Enter correct seats number");
            //}
            if (string.IsNullOrWhiteSpace(FirstName)|| string.IsNullOrWhiteSpace(LastName)|| string.IsNullOrWhiteSpace(PassportNum))
            {
                await messageService.ShowAsync("Error", "All fields shall be filled.");
                
            }
            else if (!long.TryParse(PassportNum, out passportNum))
            {
                await messageService.ShowAsync("Error", "Enter correct passport number.");
            }
            else if (await sql.AddPassengerAsync(FirstName, LastName, passportNum, Gender, BirthDate.DateTime))
            {
                await messageService.ShowAsync("Success", "Passenger was successfully added.");
                NavigationService.GoBack();
            }
            else
            {
                await messageService.ShowAsync("Error", "Something went wrong");
            }
            GetPassengerDictionary();
            IsLoading = false;
        }

        private async void GetPassengerDictionary()
        {
            var sql = ServiceLocator.Locator.Get<ISqlLinkService>();
            IsLoading = true;
            PassengerDictionary = await sql.GetPassengerDictionaryAsync();
            IsLoading = false;
        }
        private async void DeletePassengerCommandExecute()
        {
            var sql = ServiceLocator.Locator.Get<ISqlLinkService>();
            var messageService = ServiceLocator.Locator.Get<IMessageService>();
            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }
            IsLoading = true;
            if (string.IsNullOrWhiteSpace(SelectedPassengerId))
                await messageService.ShowAsync("Error", "Please, select passenger for removal.");
            else if (await sql.RemovePassengerAsync(SelectedPassengerId))
            {
                await messageService.ShowAsync("Success", "Passenger has been removed.");
            }
            else
            {
                await messageService.ShowAsync("Error", "Could not remove passenger.");
            }
            GetPassengerDictionary();
            IsLoading = false;
        }
        #endregion
    }
}

