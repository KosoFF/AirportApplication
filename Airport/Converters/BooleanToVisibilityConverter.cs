using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Airport.Converters
{
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        #region Properties

        public bool Invert { get; set; }

        #endregion

        #region IValueConverter Members

        /// <summary>
        ///     Convert Boolean or Nullable to Visibility
        /// </summary>
        /// <param name="value">bool or Nullable</param>
        /// <param name="targetType">Visibility</param>
        /// <param name="parameter">null</param>
        /// <param name="language">null</param>
        /// <returns>Visible or Collapsed</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var bValue = false;
            if (value is bool)
                bValue = (bool) value;
            if (Invert)
                bValue = !bValue;
            return bValue ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        ///     Convert Visibility to Boolean
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value is Visibility && (Visibility) value == Visibility.Visible;
        }

        #endregion
    }
}