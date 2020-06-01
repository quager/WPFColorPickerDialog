using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ColorPickerDialog
{
    public abstract class Bindable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool SetProperty<T>(ref T value, T source, [CallerMemberName]string name = null)
        {
            if ((value != null && value.Equals(source)) || (value == null && source == null))
                return false;

            value = source;
            OnPropertyChanged(name);
            return true;
        }
    }
}
