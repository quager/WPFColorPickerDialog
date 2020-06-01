using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ColorPickerDialog
{
    public class HueField : Field
    {
        public HueField(Size pickerCenterPointOffset, IColorInfo colorInfo) : base(pickerCenterPointOffset, colorInfo)
        {
        }

        public override void Draw()
        {
            if (double.IsNaN(Width) || double.IsNaN(Height) || Width == 0 || Height == 0)
                return;

            WriteableBitmap bitmap = new WriteableBitmap((int)Width, (int)Height, 96, 96, PixelFormats.Bgr24, null);
            int bytesPerPixel = bitmap.Format.BitsPerPixel / 8;
            int stride = bitmap.PixelWidth * bytesPerPixel + (bitmap.PixelWidth * bytesPerPixel % 4);
            Int32Rect rect = new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
            byte[] pixels = new byte[bitmap.PixelWidth * bitmap.PixelHeight * bytesPerPixel];
            double delta = 360.0 / (bitmap.PixelHeight - 1);

            for (int y = 0; y < bitmap.PixelHeight; y++)
            {
                double hue = y * delta;
                Color c = ColorHelper.FromHue(hue);

                for (int x = 0; x < bitmap.PixelWidth; x++)
                {
                    int pixelOffset = (x + y * bitmap.PixelWidth) * bytesPerPixel;

                    pixels[pixelOffset] = c.B;
                    pixels[pixelOffset + 1] = c.G;
                    pixels[pixelOffset + 2] = c.R;
                }
            }

            bitmap.WritePixels(rect, pixels, stride, 0);
            bitmap.Freeze();
            Image = bitmap;
        }

        protected override void UpdatePickerPosition()
        {
            if (!NeedUpdatePickerPosition)
                return;

            NeedUpdateColor = false;
            double y = ColorInfo.HsvValue.Hue * Height / 360.0;
            SetPickerPosition(new Point(0, y));
            NeedUpdateColor = true;
        }

        protected override void PickerPositionChanged()
        {
            if (!NeedUpdateColor)
                return;

            NeedUpdatePickerPosition = false;
            double hueValue = PickerPosition.Y * 360.0 / Height;
            ColorInfo.UpdateHue(hueValue);
            NeedUpdatePickerPosition = true;
        }
    }
}
