//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data
{
  internal class Ball : IBall, IDisposable
  {
    private bool _isDisposed = false;
    private Vector _position;
    private double _boardWidth;
    private double _boardHeight;
    private double _radius;

    internal Ball(Vector initialPosition, Vector initialVelocity, double boardWidth, double boardHeight, double radius)
    {
      _position = initialPosition;
      Velocity = initialVelocity;
      _boardWidth = boardWidth;
      _boardHeight = boardHeight;
      _radius = radius;
    }

    public event EventHandler<IVector>? NewPositionNotification;

    public IVector Velocity { get; set; }
    public IVector Position => _position;

    private void RaiseNewPositionChangeNotification()
    {
      NewPositionNotification?.Invoke(this, _position);
    }

    internal void StartMoving()
    {
      Task.Run(async () =>
      {
        while (!_isDisposed)
        {
          Move();
          await Task.Delay(10);
        }
      });
    }

    internal void Move()
    {
      double newX = _position.x + Velocity.x;
      double newY = _position.y + Velocity.y;
      double newVX = Velocity.x;
      double newVY = Velocity.y;

      if ((newX <= _radius && newVX < 0) || (newX >= _boardWidth - _radius && newVX > 0))
      {
        newVX = -newVX;
      }

      if ((newY <= _radius && newVY < 0) || (newY >= _boardHeight - _radius && newVY > 0))
      {
        newVY = -newVY;
      }

      Velocity = new Vector(newVX, newVY);
      _position = new Vector(newX, newY);
      RaiseNewPositionChangeNotification();
    }

    public void Dispose()
    {
      _isDisposed = true;
    }
  }
}