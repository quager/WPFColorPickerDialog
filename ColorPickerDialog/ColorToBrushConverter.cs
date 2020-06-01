﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ColorPickerDialog
{
    [ValueConversion(typeof(Color), typeof(Brush))]
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            new SolidColorBrush((Color)value);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
