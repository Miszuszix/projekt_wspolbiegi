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
    public object BallLock { get; } = new object();
    private readonly ILogger? _logger;
    private readonly bool _isInteractive;

    internal Ball(Vector position, Vector velocity, double boardWidth, double boardHeight, double radius, ILogger? logger = null, bool isInteractive = false)
    {
      _position = position;
      Velocity = velocity;
      _boardWidth = boardWidth;
      _boardHeight = boardHeight;
      _radius = radius;
      _logger = logger;
      _isInteractive = isInteractive;
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
      if (_isInteractive) return;
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
      lock (BallLock)
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
      }

      string logMessage = $"{this.GetHashCode()}; {_position.x:F2}; {_position.y:F2}; {Velocity.x:F2}; {Velocity.y:F2}";
      _logger?.Log(LogLevel.Info, logMessage);

      RaiseNewPositionChangeNotification();
    }

    internal void SetPosition(double x, double y)
    {
      lock (BallLock)
      {
        _position = new Vector(x, y);
      }

      string logMessage = $"GRACZ {this.GetHashCode()}; {_position.x:F2}; {_position.y:F2}; {Velocity.x:F2}; {Velocity.y:F2}";
      _logger?.Log(LogLevel.Info, logMessage);

      RaiseNewPositionChangeNotification();
    }

    public void Dispose()
    {
      _isDisposed = true;
    }
  }
}