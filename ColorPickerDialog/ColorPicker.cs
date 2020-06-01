using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static ColorPickerDialog.ColorHelper;

namespace ColorPickerDialog
{
    public class ColorPicker : Bindable, IColorInfo
    {
        private static readonly Color DefaultColor = Colors.Red;
        private string _colorString;
        private bool _changed;
        private Color _color;
        private byte _alpha;
        private byte _green;
        private byte _blue;
        private byte _red;

        public event Action ColorChanged;

        #region Properties

        public byte Red
        {
            get => _red;
            set
            {
                if (SetProperty(ref _red, value))
                    OnColorComponentChanged();
            }
        }

        public byte Green
        {
            get => _green;
            set
            {
                if (SetProperty(ref _green, value))
                    OnColorComponentChanged();
            }
        }

        public byte Blue
        {
            get => _blue;
            set
            {
                if (SetProperty(ref _blue, value))
                    OnColorComponentChanged();
            }
        }

        public byte Alpha
        {
            get => _alpha;
            set
            {
                if (SetProperty(ref _alpha, value))
                    OnColorComponentChanged();
            }
        }

        public HsvColor HsvValue { get; private set; }

        public Color ColorValue
        {
            get => _color;
            private set => SetProperty(ref _color, value);
        }

        public string ColorString
        {
            get => _colorString;
            set
            {
                if (SetColorByText(value))
                {
                    if (SetProperty(ref _colorString, value.ToUpper()))
                        OnColorComponentChanged();
                }
            }
        }

        public ColorField ColorField { get; }

        public HueField HueField { get; }

        #endregion

        public ColorPicker(Size colorPickerCenterPointOffset, Size huePickerCenterPointOffset) :
            this(DefaultColor, colorPickerCenterPointOffset, huePickerCenterPointOffset)
        {
        }

        public ColorPicker(Color initialColor, Size colorPickerCenterPointOffset, Size huePickerCenterPointOffset)
        {
            ColorField = new ColorField(colorPickerCenterPointOffset, this);
            HueField = new HueField(huePickerCenterPointOffset, this);
            UpdateColor(initialColor);
        }

        public void UpdateColor(Color value)
        {
            double oldHue = HsvValue?.Hue ?? 0;
            ColorValue = value;
            HsvValue = value.ToHsv();
            _colorString = value.ToString();
            OnPropertyChanged(nameof(ColorString));

            if (!ColorField.IsMoving && !HueField.IsMoving)
                ColorChanged?.Invoke();

            if (!_changed)
                UpdateRgba();

            double newHue = HsvValue.Hue;
            if (newHue != oldHue)
                ColorField.Draw();
        }

        public bool SetColorByText(string colorText)
        {
            try
            {
                _changed = true;
                ColorValue = (Color)ColorConverter.ConvertFromString(colorText);
                UpdateRgba();
                ColorChanged?.Invoke();
                _changed = false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void UpdateHue(double hue)
        {
            Color newValue = FromHue(hue);
            UpdateColor(newValue);
        }

        public void OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                StopMoving();
        }

        public void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            StopMoving();
        }

        private void StopMoving()
        {
            HueField.StopPickerMoving();
            ColorField.StopPickerMoving();
        }

        private void OnColorComponentChanged()
        {
            if (_changed || ColorField.IsMoving)
                return;

            _changed = true;
            UpdateColor(Color.FromArgb(Alpha, Red, Green, Blue));
            _changed = false;
        }

        private void UpdateRgba()
        {
            Red = ColorValue.R;
            Green = ColorValue.G;
            Blue = ColorValue.B;
            Alpha = ColorValue.A;
        }
    }
}
