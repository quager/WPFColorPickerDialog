using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ColorPickerDialog
{
    public partial class ColorDialog : Window
    {
        public Color SelectedColor { get; private set; }

        public ColorPicker Picker { get; }

        public ColorDialog()
        {
            InitializeComponent();
            DataContext = this;

            Picker = new ColorPicker(new Size(5, 5), new Size(0, 0));

            Application.Current.MainWindow.MouseMove += OnMouseMove;
            Application.Current.MainWindow.MouseUp += Picker.OnMouseUp;
            Application.Current.MainWindow.MouseEnter += Picker.OnMouseEnter;
        }

        private void UpdateStringValue(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && e.KeyboardDevice.Modifiers == ModifierKeys.None && sender is TextBox tb)
                tb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            Point huePickerPosition = e.GetPosition(HueImage);
            Picker.HueField.MovePicker(huePickerPosition);

            Point colorPickerPosition = e.GetPosition(ColorImage);
            Picker.ColorField.MovePicker(colorPickerPosition);
        }

        private void HueField_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(sender as IInputElement);
            Picker.HueField.StartPickerMoving(position);
            //Picker.ColorField.IsFreezed = true;
        }

        private void ColorField_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(sender as IInputElement);
            Picker.ColorField.StartPickerMoving(position);
            //Picker.HueField.IsFreezed = true;
        }

        private void ColorField_SizeChanged(object sender, SizeChangedEventArgs e) => Picker.ColorField.SizeChanged(e.NewSize);

        private void HueField_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Size newSize = new Size(HueImage.Width, e.NewSize.Height - HueImage.Margin.Top - HueImage.Margin.Bottom);
            Picker.HueField.SizeChanged(newSize);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            SelectedColor = Picker.ColorValue;
            //DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //DialogResult = false;
            Close();
        }
    }
}
