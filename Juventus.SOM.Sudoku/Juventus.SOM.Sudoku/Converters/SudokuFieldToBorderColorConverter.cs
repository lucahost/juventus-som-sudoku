using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Juventus.SOM.Sudoku.Models;
using Xamarin.Forms;

namespace Juventus.SOM.Sudoku.Converters
{
    public class SudokuFieldToBorderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Field field)
            {
                if (field.X == 1 && field.Y == 1)
                {
                    return Color.Black;
                }
                if (field.X % 3 == 0)
                {
                    return Color.Black;
                }

                if (field.Y % 3 == 0)
                {
                    return Color.Black;
                }
            }

            return Color.DarkGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
