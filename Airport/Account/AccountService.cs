using System;
using System.Threading.Tasks;
using Airport.Account;
using Airport.Core.DependencyInjection;
using Airport.Core.Helpers;
using Airport.Core.MessageServices;
using Airport.Core.Models;
using Airport.Core.Observable;
using Airport.SqlReference;

namespace Airport.Account
{
    public class AccountService : ObservableObject, IAccountService
    {
        //private string _userName;
        //public string UserName {
        //    get { return this._userName; }
        //    set { Set(ref this._userName, value); }
        //}

        //private string _email;
        //public string Email
        //{
        //    get { return this._email; }
        //    set { Set(ref this._email, value); }
        //}

        //private string _passwordHash;
        //public string PasswordHash
        //{
        //    get { return this._passwordHash; }
        //    set { Set(ref this._passwordHash, value); }
        //}

        private bool _checkInProgress;
        private bool _isLoggedIn;

        private Account _account;

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                Set(ref _isLoggedIn, value);
                OnAccountStatusChanged();
            }
        }

        public Account Account
        {
            get { return _account; }
            set { Set(ref _account, value); }
        }

        public Action AccountStatusChanged { get; set; }

        public async Task SignUpAsync(Account account)
        {
            account.PasswordHash = Helpers.ComputeMd5(account.PasswordHash);
            var sqlService = new ServiceClient();
            await sqlService.SignUpAsync(new Check_in()
            {
                Login = account.UserName,
                PasswordHash = account.PasswordHash,
                FirstName = account.FirstName,
                LastName = account.LastName
            });
        }

        public async Task LoginAsync(string login, string pass)
        {
            var password = Helpers.ComputeMd5(pass);
            if (_checkInProgress) return;
            _checkInProgress = true;
            var messageService = ServiceLocator.Locator.Get<IMessageService>();

            var sqlService = new ServiceClient();

            var response = await sqlService.LoginAsync(login, password);
            if (response == null)
            {
                await messageService.ShowAsync("Error", "Invalid login or password");
                _checkInProgress = false;
                return;
            }
            if (response.Login == "Connection error")
            {
#if DEBUG

                await
                    messageService.ShowAsync("Error",
                        "There must be a problem with connection to database. \n Anyway, welcome.");

#else
                await messageService.ShowAsync("Error", "There must be a problem with connection to database. ");
                return;
#endif
            }
            //User = new User
            //{
            //    Email = response?.Email,
            //    PasswordHash = response?.PasswordHash,
            //    Id = response?.Id,
            //    UserName = response?.UserName,
            //    PhoneNumber = response?.PhoneNumber,
            //    PhoneNumberConfirmed = response.PhoneNumberConfirmed,
            //    AccessFailedCount = response.AccessFailedCount,
            //    EmailConfirmed = response.EmailConfirmed,
            //    LockoutEnabled = response.LockoutEnabled,
            //    TwoFactorEnabled = response.TwoFactorEnabled,
            //    SecurityStamp = response?.SecurityStamp
            //};
#if DEBUG
            //await messageService.ShowAsync("Success", "Hi, " + User.UserName);
#endif
            Account = new Account()
            {
                FirstName = response.FirstName,
                LastName = response.LastName,
                PasswordHash = response.PasswordHash,
                UserId = response.Id,
                UserName = response.Login
            };
            IsLoggedIn = true;
            OnAccountStatusChanged();
            _checkInProgress = false;
        }

        public async Task LogoutAsync()
        {
            IsLoggedIn = false;
            Account = new Account();
        }

        public void OnAccountStatusChanged()
        {
            var handler = AccountStatusChanged;
            handler?.Invoke();
        }
    }
}