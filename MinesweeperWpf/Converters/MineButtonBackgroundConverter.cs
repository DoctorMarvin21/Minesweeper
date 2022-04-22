using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MinesweeperWpf
{
    public class MineButtonBackgroundConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is bool isOpened  && values[1] is bool hasMine)
            {
                if (isOpened)
                {
                    if (hasMine)
                    {
                        return Brushes.OrangeRed;
                    }
                    else
                    {
                        return Brushes.White;
                    }
                }
                else
                {
                    return Brushes.LightGray;
                }
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
