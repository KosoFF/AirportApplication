using Windows.Security.Credentials;
using Airport.Account;
using Airport.Core.Commands;
using Airport.Core.DependencyInjection;
using Airport.Core.Helpers;
using Airport.Core.MessageServices;
using System;

namespace Airport.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        #region Ctor

        public LoginViewModel()
        {
            
            LoginCommand = new RelayCommand(LoginCommandExecute, () => !IsLoading);
            BackCommand = new RelayCommand(() => NavigationService.GoBack());
            SignInCommand= new RelayCommand(SigninCommandExecute, () => !IsLoading);
            SignInWithVkCommand = new RelayCommand(SigninWithVkCommandExecute, ()=>!IsLoading);
#if DEBUG
            Username = "abraam";
            Password = "urUkdrXi";
#endif
        }

       
        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            SignInCommand.RaiseCanExecuteChanged();
            SignInWithVkCommand.RaiseCanExecuteChanged();
            LoginCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Fields

        private string _username;
        private string _password;
        private bool _rememberMe;

        #endregion

        #region Properties

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

        public bool RememberMe
        {
            get { return _rememberMe; }
            set { Set(ref _rememberMe, value); }
        }

        public RelayCommand SignInCommand { get; set; }

        public RelayCommand LoginCommand { get; set; }

        public RelayCommand SignInWithVkCommand { get; set; }

        public RelayCommand BackCommand { get; set; }

        #endregion

        #region Private methods

        private void SigninWithVkCommandExecute()
        {
            //NavigationService.Navigate(ViewLocator.ForgotPassword);
        }
        private void SigninCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.SignUp);
        }

        private async void LoginCommandExecute()
        {
            var messageService = ServiceLocator.Locator.Get<IMessageService>();
            var accountService = ServiceLocator.Locator.Get<IAccountService>();
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await messageService.ShowAsync("Message", "Please fill all empty input fields");
                return;
            }

            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }

            IsLoading = true;

            if (RememberMe)
            {
                var passwordVault = new PasswordVault();
                //var credentials = new PasswordCredential(ApplicationKeys.QMunicateCredentials, Email, Password);
                //passwordVault.Add(credentials);
            }
            await accountService.LoginAsync(Username, Password);
            if(accountService.IsLoggedIn)
            NavigationService.NavigateWithoutHistory(ViewLocator.Control);
            IsLoading = false;


            // else await Helpers.ShowErrors(response.Errors, messageService);
        }

        #endregion
    }
}