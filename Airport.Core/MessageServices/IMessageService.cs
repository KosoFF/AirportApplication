using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airport.Core.MessageServices
{
    public interface IMessageService
    {
        Task ShowAsync(string title, string message);

        Task ShowAsync(string title, string message, IEnumerable<DialogCommand> commands);
    }
}