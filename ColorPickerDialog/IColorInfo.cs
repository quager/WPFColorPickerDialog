using System;
using System.Windows.Media;
using static ColorPickerDialog.ColorHelper;

namespace ColorPickerDialog
{
    public interface IColorInfo
    {
        byte Red { get; set; }

        byte Green { get; set; }

        byte Blue { get; set; }

        byte Alpha { get; set; }

        Color ColorValue { get; }

        HsvColor HsvValue { get; }

        string ColorString { get; set; }

        event Action ColorChanged;

        void UpdateHue(double hue);

        void UpdateColor(Color value);
    }
}
