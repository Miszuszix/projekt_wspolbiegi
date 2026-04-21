//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2023, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//  by introducing yourself and telling us what you do with this community.
//_____________________________________________________________________________________________________________________________________

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
        private const double ScalingFactor = 1.0; 

        public event PropertyChangedEventHandler? PropertyChanged;

        public ModelBall(double x, double y, double radius, LogicIBall underneathBall)
        {
            _radius = radius;
            
            Diameter = (radius * 2) * ScalingFactor;
            Left = (x - _radius) * ScalingFactor;
            Top = (y - _radius) * ScalingFactor;
            
            underneathBall.NewPositionNotification += NewPositionNotification;
        }

        public double Left
        {
            get => _left;
            private set 
            { 
                if (_left != value) 
                { 
                    _left = value; 
                    OnPropertyChanged();
                } 
            }
        }

        public double Top
        {
            get => _top;
            private set 
            { 
                if (_top != value) 
                { 
                    _top = value; 
                    OnPropertyChanged();
                } 
            }
        }

        public double Diameter { get; }

        private void NewPositionNotification(object? sender, IPosition e)
        {
            Left = (e.x - _radius) * ScalingFactor;
            Top = (e.y - _radius) * ScalingFactor;
        }
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}