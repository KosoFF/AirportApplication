using System;
using Windows.UI.Xaml.Data;
using Airport.Core.Helpers;

namespace Airport.Converters
{
    public sealed class StringToMd5Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var bValue = (string) value;
            return Helpers.ComputeMd5(bValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}