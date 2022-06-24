using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace QuizMastersApprenticeApp.Converters
{
    [ValueConversion(typeof(double), typeof(double))]
    public sealed class SubtractVerticalScrollbarWidthConverter : IValueConverter
    {
        public SubtractVerticalScrollbarWidthConverter()
        {
        }

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var scrollBarWidth = (double)36; //This is hack figure it out later - SystemParameters.VerticalScrollBarWidth;
            var width = (double)value;

            return (scrollBarWidth < width) ? width - scrollBarWidth : scrollBarWidth;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
