using System;
using System.Threading.Tasks;
using Airport.Core.Models;

namespace Airport.Account
{
    public interface IAccountService
    {
        bool IsLoggedIn { get; set; }
        Account Account { get; set; }
        Action AccountStatusChanged { get; set; }
        Task LoginAsync(string login, string password);
        Task LogoutAsync();
        Task SignUpAsync(Account account);
    }
}