using System.Windows;
using System.Windows.Media;

namespace ColorPickerDialog
{
    public class ColorField : Field
    {
        private Brush _pickerStroke;

        public Brush PickerStroke
        {
            get => _pickerStroke;
            private set => SetProperty(ref _pickerStroke, value);
        }

        public ColorField(Size pickerCenterPointOffset, IColorInfo colorInfo) : base(pickerCenterPointOffset, colorInfo)
        {
        }

        public override void Draw()
        {
            if (double.IsNaN(Width) || double.IsNaN(Height) || Width == 0 || Height == 0)
                return;

            double value = 1.0;
            double deltaValue = 1.0 / Height;
            DrawingGroup group = new DrawingGroup();

            for (int y = 0; y < Height; y++)
            {
                Rect rect = new Rect(new Point(0, y), new Point(Width, Height));
                Color start = ColorHelper.FromHsv(ColorInfo.HsvValue.Hue, 0, value);
                Color end = ColorHelper.FromHsv(ColorInfo.HsvValue.Hue, 1, value);

                LinearGradientBrush linearGradientBrush = new LinearGradientBrush(start, end, new Point(0, 0), new Point(1, 0));
                GeometryDrawing line = new GeometryDrawing(linearGradientBrush, null, new RectangleGeometry(rect));
                group.Children.Add(line);
                value -= deltaValue;
            }

            DrawingImage img = new DrawingImage(group);
            Image = img;
            Image.Freeze();
        }

        protected override void UpdatePickerPosition()
        {
            if (!NeedUpdatePickerPosition)
                return;

            NeedUpdateColor = false;
            double x = ColorInfo.HsvValue.Saturation * Width;
            double y = (1.0 - ColorInfo.HsvValue.Value) * Height;
            ResetPickerColor();
            SetPickerPosition(new Point(x, y));
            NeedUpdateColor = true;
        }

        protected override void PickerPositionChanged()
        {
            if (!NeedUpdateColor)
                return;

            NeedUpdatePickerPosition = false;
            double saturation = PickerPosition.X / Width;
            double value = 1.0 - PickerPosition.Y / Height;
            ResetPickerColor();
            ColorInfo.UpdateColor(ColorHelper.FromHsv(ColorInfo.HsvValue.Hue, saturation, value));
            NeedUpdatePickerPosition = true;
        }

        private void ResetPickerColor() => PickerStroke = 1.0 - PickerPosition.Y / Height > 0.4 ? Brushes.Black : Brushes.White;
    }
}
