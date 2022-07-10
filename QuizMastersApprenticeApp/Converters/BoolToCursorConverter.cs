using System;
using System.Windows.Data;
using System.Windows.Input;

namespace QuizMastersApprenticeApp.Converters
{
    [ValueConversion(typeof(bool), typeof(Cursor))]
    public class BoolToCursorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value == true)
            {
                return Cursors.Wait;
            }
            else
            {
                return Cursors.Arrow;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
