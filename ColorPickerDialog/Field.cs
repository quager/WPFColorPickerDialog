using System.Windows;
using System.Windows.Media;

namespace ColorPickerDialog
{
    public abstract class Field : Bindable
    {
        private Point _pickerCenterPosition;
        private ImageSource _image;

        public bool IsMoving { get; private set; }

        protected bool NeedUpdatePickerPosition = true;
        protected bool NeedUpdateColor = true;
        protected Size PickerCenterPointOffset;
        protected IColorInfo ColorInfo;
        protected double Height;
        protected double Width;

        protected Point PickerPosition { get; private set; }

        public Point PickerCenterPosition
        {
            get => _pickerCenterPosition;
            private set
            {
                if (SetProperty(ref _pickerCenterPosition, value))
                    PickerPositionChanged();
            }
        }

        public ImageSource Image
        {
            get => _image;
            protected set => SetProperty(ref _image, value);
        }

        public Field(Size pickerCenterPointOffset, IColorInfo colorInfo)
        {
            ColorInfo = colorInfo;
            PickerCenterPointOffset = pickerCenterPointOffset;
            ColorInfo.ColorChanged += CheckUpdatePickerPosition;
        }

        public abstract void Draw();

        public void SizeChanged(Size newSize)
        {
            Height = newSize.Height;
            Width = newSize.Width;
            Draw();
            UpdatePickerPosition();
        }

        public void StartPickerMoving(Point newPosition)
        {
            IsMoving = true;
            SetPickerPosition(newPosition);
        }

        public void StopPickerMoving()
        {
            IsMoving = false;
        }

        public void MovePicker(Point newPosition)
        {
            if (!IsMoving)
                return;

            if (newPosition.X < 0)
                newPosition.X = 0;

            if (newPosition.X >= Width)
                newPosition.X = Width;

            if (newPosition.Y < 0)
                newPosition.Y = 0;

            if (newPosition.Y >= Height)
                newPosition.Y = Height;

            SetPickerPosition(newPosition);
        }

        protected void SetPickerPosition(Point newPosition)
        {
            PickerPosition = newPosition;
            newPosition.Offset(-PickerCenterPointOffset.Width, -PickerCenterPointOffset.Height);
            PickerCenterPosition = newPosition;
        }

        protected abstract void UpdatePickerPosition();

        protected abstract void PickerPositionChanged();

        private void CheckUpdatePickerPosition()
        {
            if (!IsMoving)
                UpdatePickerPosition();
        }
    }
}
