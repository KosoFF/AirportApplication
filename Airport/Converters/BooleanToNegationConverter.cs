using System;
using Windows.UI.Xaml.Data;

namespace Airport.Converters
{
    public sealed class BooleanToNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var bValue = (bool) value;
            return !bValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var bValue = (bool)value;
            return !bValue;
        }
    }
}