using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ColorPickerDialog
{
    public partial class NumericUpDown : UserControl
    {
        /// <summary>
        /// Identifies the Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(byte), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnValueChanged), new CoerceValueCallback(CoerceValue)));

        /// <summary>
        /// Gets or sets the value assigned to the control.
        /// </summary>
        public byte Value
        {
            get => (byte)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<byte>), typeof(NumericUpDown));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<byte> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        private const byte MinValue = 0, MaxValue = 255;

        public NumericUpDown()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the ValueChanged event.</param>
        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<byte> args)
        {
            RaiseEvent(args);
        }

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown control = (NumericUpDown)obj;

            RoutedPropertyChangedEventArgs<byte> e = new RoutedPropertyChangedEventArgs<byte>(
                (byte)args.OldValue, (byte)args.NewValue, ValueChangedEvent);

            control.OnValueChanged(e);
        }

        private static object CoerceValue(DependencyObject element, object value)
        {
            byte newValue = (byte)value;
            NumericUpDown control = (NumericUpDown)element;

            newValue = Math.Max(MinValue, Math.Min(MaxValue, newValue));
            return newValue;
        }

        private void IncVersion(object sender, RoutedEventArgs e)
        {
            if (Value >= MaxValue)
                return;

            Value++;
        }

        private void DecVersion(object sender, RoutedEventArgs e)
        {
            if (Value <= MinValue)
                return;

            Value--;
        }

        private void UpdateValue(string textValue)
        {
            int intValue = Convert.ToInt32(textValue);
            Value = (byte)Math.Min(intValue, MaxValue);
            ValueControl.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        private void UpdateStringValue(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
                UpdateValue(tb.Text);
        }

        private void UpdateStringValue(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers != ModifierKeys.None)
            {
                e.Handled = true;
                return;
            }

            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                return;

            if (e.Key == Key.Return && sender is TextBox tb)
            {
                UpdateValue(tb.Text);
                return;
            }

            ValueControl.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            e.Handled = true;
        }
    }
}
