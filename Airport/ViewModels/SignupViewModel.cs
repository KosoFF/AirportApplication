using Airport.Account;
using Airport.Core.Commands;
using Airport.Core.DependencyInjection;
using Airport.Core.Helpers;
using Airport.Core.MessageServices;
using Airport.Core.Models;

namespace Airport.ViewModels
{
    public class SignupViewModel : ViewModel
    {
        #region Ctor

        public SignupViewModel()
        {
            messageService = ServiceLocator.Locator.Get<IMessageService>();

            SignUpCommand = new RelayCommand(SignUpCommandExecute, () => !IsLoading);
            BackCommand = new RelayCommand(() => NavigationService.GoBack());
        }

        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            SignUpCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Private methods

        private async void SignUpCommandExecute()
        {
            if (string.IsNullOrWhiteSpace(Firstname) || string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password))
            {
                await messageService.ShowAsync("Message", "Please fill all empty input fields");
                return;
            }
            if (!Password.Equals(RepeatPassword))
            {
                await messageService.ShowAsync("Error", "Passwords are not equal.");
                return;
            }

            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }

            IsLoading = true;

            var accountService = ServiceLocator.Locator.Get<IAccountService>();
            await accountService.SignUpAsync(new Account.Account()
            {
                UserName = Username,
                PasswordHash = Password,
                FirstName = Firstname,
                LastName = Secondname
                
            });
            IsLoading = false;
            NavigationService.Navigate(ViewLocator.Login);
        }

        #endregion

        #region Fields

        private string _firstname;
        private string _username;
        private string _password;
        private string _secondname;
        private string _repeatPassword;

        private readonly IMessageService messageService;

        #endregion

        #region Properties

        public string Secondname
        {
            get {  return _secondname;}
            set { Set(ref _secondname, value); }
        }
        public string Firstname
        {
            get { return _firstname; }
            set { Set(ref _firstname, value); }
        }

        public string Username
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }

        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }


        public RelayCommand SignUpCommand { get; set; }

        public RelayCommand BackCommand { get; set; }

        public string RepeatPassword
        {
            get { return _repeatPassword; }
            set { Set(ref _repeatPassword, value); }
        }

        #endregion
    }
}