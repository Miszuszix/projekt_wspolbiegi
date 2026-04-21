using System.ComponentModel;
using System.Runtime.CompilerServices;
using TP.ConcurrentProgramming.BusinessLogic;
using LogicIBall = TP.ConcurrentProgramming.BusinessLogic.IBall;

namespace TP.ConcurrentProgramming.Presentation.Model
{
    internal class ModelBall : IBall
    {
        private double _left;
        private double _top;
        private readonly double _radius;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ModelBall(double x, double y, double radius, LogicIBall underneathBall)
        {
            _radius = radius;
            Diameter = radius * 2;
            Left = x - _radius;
            Top = y - _radius;
            
            underneathBall.NewPositionNotification += NewPositionNotification;
        }

        public double Left
        {
            get => _left;
            private set { if (_left != value) { _left = value; OnPropertyChanged(); } }
        }

        public double Top
        {
            get => _top;
            private set { if (_top != value) { _top = value; OnPropertyChanged(); } }
        }

        public double Diameter { get; }

        private void NewPositionNotification(object? sender, IPosition e)
        {
            Left = e.x - _radius;
            Top = e.y - _radius;
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}