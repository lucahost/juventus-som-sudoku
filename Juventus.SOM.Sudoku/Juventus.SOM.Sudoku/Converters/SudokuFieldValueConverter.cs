using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Juventus.SOM.Sudoku.Converters
{
    public class SudokuFieldValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int iValue)
            {
                if (iValue == -1)
                {
                    return "";
                }
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                if (string.IsNullOrEmpty(strValue))
                {
                    return -1;
                }
                if (int.TryParse(strValue, out int iValue))
                {
                    return iValue;
                }
                else
                {
                    throw new InvalidCastException("Cannot cast input to integer");
                }
            }
            return value;
        }
    }
}
