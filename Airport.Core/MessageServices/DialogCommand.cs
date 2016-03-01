using System.Windows.Input;

namespace Airport.Core.MessageServices
{
    public class DialogCommand
    {
        #region Ctor

        /// <param name="label">The text for the command.</param>
        /// <param name="command">The command.</param>
        /// <param name="isDefault">A value that indicates whether a command is a Default button.</param>
        /// <param name="isCancel">A value that indicates whether a command is a Cancel button.</param>
        /// <param name="tag">The tag for the command.</param>
        public DialogCommand(string label, ICommand command, bool isDefault = false, bool isCancel = false,
            object tag = null)
        {
            Label = label;
            Command = command;
            IsDefault = isDefault;
            IsCancel = isCancel;
            Tag = tag;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the caption for the command.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        ///     Gets or sets the command.
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        ///     Gets or sets a value that indicates whether a command is a Default button.
        /// </summary>
        public bool IsDefault { get; private set; }

        /// <summary>
        ///     Gets or sets a value that indicates whether a command is a Cancel button.
        /// </summary>
        public bool IsCancel { get; private set; }

        /// <summary>
        ///     Gets or sets the tag for the command.
        /// </summary>
        public object Tag { get; private set; }

        #endregion
    }
}