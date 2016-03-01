using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Airport.Core.MessageServices
{
    public class MessageService : IMessageService
    {
        #region Fields

        private static bool _showing;

        #endregion

        public async Task ShowAsync(string title, string message)
        {
            await ShowAsync(title, message, null);
        }

        public async Task ShowAsync(string title, string message, IEnumerable<DialogCommand> commands)
        {
            if (_showing) return;
            _showing = true;
            var messageDialog = new MessageDialog(message ?? string.Empty, title);
            if (commands != null)
            {
                var uiCommands = commands.Select(c => new UICommand(c.Label, command => c.Command.Execute(null), c.Tag));
                foreach (var command in uiCommands)
                {
                    messageDialog.Commands.Add(command);
                }
            }
            await messageDialog.ShowAsync();
            _showing = false;
        }
    }
}