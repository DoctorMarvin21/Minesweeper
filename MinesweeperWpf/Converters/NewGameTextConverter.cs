using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MinesweeperWpf
{
    public class NewGameTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GameState state)
            {
                switch (state)
                {
                    case GameState.Failed:
                        return "🙁";
                    case GameState.InProgress:
                        return "🙂";
                    default:
                        return "😀";
                }
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
