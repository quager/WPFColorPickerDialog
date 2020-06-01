using System;
using System.Windows.Media;

namespace ColorPickerDialog
{
    public static class ColorHelper
    {
        public class HsvColor
        {
            public double Hue { get; set; }
            public double Saturation { get; set; }
            public double Value { get; set; }
        }

        public static double GetHue(Color colorValue)
        {
            double hue = 0;
            byte red = colorValue.R;
            byte green = colorValue.G;
            byte blue = colorValue.B;

            double max = Math.Max(red, Math.Max(green, blue));
            double min = Math.Min(red, Math.Min(green, blue));
            double c = (byte)(max - min);

            if (c == 0)
                return 0;

            if (max == red)
                hue = (green - blue) / c;

            if (max == green)
                hue = (blue - red) / c + 2;

            if (max == blue)
                hue = (red - green) / c + 4;

            hue *= 60;
            if (hue < 0)
                hue += 360;

            return hue;
        }

        public static HsvColor ToHsv(this Color color)
        {
            double normCoef = 1.0 / 255.0;
            var red = normCoef * color.R;
            var green = normCoef * color.G;
            var blue = normCoef * color.B;
            var max = Math.Max(Math.Max(red, green), blue);
            var min = Math.Min(Math.Min(red, green), blue);
            var chroma = max - min;
            double hue;

            if (chroma == 0)
                hue = 0;
            else
            if (max == red)
                hue = (((green - blue) / chroma) + 6) % 6;
            else
            if (max == green)
                hue = 2 + ((blue - red) / chroma);
            else
                hue = 4 + ((red - green) / chroma);

            double saturation = chroma == 0 ? 0 : chroma / max;
            HsvColor ret = new HsvColor
            {
                Hue = 60 * hue,
                Saturation = saturation,
                Value = max
            };

            return ret;
        }

        public static Color FromHue(double hue)
        {
            int sextant = (int)Math.Floor(hue / 60.0);

            if (hue >= 300)
                hue -= 360;

            hue /= 60.0;
            hue -= 2.0 * Math.Floor(((sextant + 1) % 6.0) / 2.0);

            double mid = (sextant % 2 == 0) ? hue : -hue;
            byte hueColor = DoubleToByte(mid * 255.0);

            switch (sextant)
            {
                case 1:
                    return Color.FromRgb(hueColor, 255, 0);
                case 2:
                    return Color.FromRgb(0, 255, hueColor);
                case 3:
                    return Color.FromRgb(0, hueColor, 255);
                case 4:
                    return Color.FromRgb(hueColor, 0, 255);
                case 5:
                    return Color.FromRgb(255, 0, hueColor);
                default:
                    return Color.FromRgb(255, hueColor, 0);
            }
        }

        public static Color FromHsv(HsvColor hsv) => FromHsv(hsv.Hue, hsv.Saturation, hsv.Value);

        public static Color FromHsv(double hue, double saturation, double value)
        {
            if (hue < 0 || hue > 360)
                hue = hue % 360;

            double h = hue / 60;
            int sextant = (int)Math.Floor(h);
            double chroma = value * saturation;
            double x = chroma * (1 - Math.Abs((h % 2) - 1));
            double add = value - chroma;
            double red;
            double green;
            double blue;

            switch (sextant)
            {
                case 0:
                    red = chroma;
                    green = x;
                    blue = 0;
                    break;
                case 1:
                    red = x;
                    green = chroma;
                    blue = 0;
                    break;
                case 2:
                    red = 0;
                    green = chroma;
                    blue = x;
                    break;
                case 3:
                    red = 0;
                    green = x;
                    blue = chroma;
                    break;
                case 4:
                    red = x;
                    green = 0;
                    blue = chroma;
                    break;
                default:
                    red = chroma;
                    green = 0;
                    blue = x;
                    break;
            }

            byte r = DoubleToByte((red + add) * 255.0);
            byte g = DoubleToByte((green + add) * 255.0);
            byte b = DoubleToByte((blue + add) * 255.0);

            return Color.FromRgb(r, g, b);
        }

        private static byte DoubleToByte(double value)
        {
            double result = Math.Min(value, 255);
            result = Math.Max(result, 0);

            return (byte)result;
        }
    }
}
