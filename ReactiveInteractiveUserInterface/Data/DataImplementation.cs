//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System.Diagnostics;

namespace TP.ConcurrentProgramming.Data
{
  internal class DataImplementation : DataAbstractAPI
  {
    internal const double BoardWidth = 400.0;
    internal const double BoardHeight = 400.0;
    internal const double BallRadius = 10.0;
    private DiagnosticLogger? _logger;
    private Ball? _interactiveBall;
    
    public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler)
    {
      if (Disposed)
        throw new ObjectDisposedException(nameof(DataImplementation));
      if (upperLayerHandler == null)
        throw new ArgumentNullException(nameof(upperLayerHandler));
        
      BallsList.Clear();

      _logger = new DiagnosticLogger("balls_diagnostic_log.txt");

      Random random = new Random();

      _interactiveBall = new Ball(new Vector(BoardWidth / 2, BoardHeight / 2), new Vector(0, 0), BoardWidth, BoardHeight, 15, _logger, true);
      BallsList.Add(_interactiveBall);

      _interactiveBall.NewPositionNotification += (sender, pos) => upperLayerHandler(pos, _interactiveBall);

      for (int i = 0; i < numberOfBalls; i++)
      {
        Vector startingPosition = new(
          random.Next((int)BallRadius, (int)(BoardWidth - BallRadius)), 
          random.Next((int)BallRadius, (int)(BoardHeight - BallRadius))
        );
        Vector startingVelocity = new((random.NextDouble() - 0.5) * 3, (random.NextDouble() - 0.5) * 3);
        
        Ball newBall = new(startingPosition, startingVelocity, BoardWidth, BoardHeight, BallRadius, _logger);
        upperLayerHandler(startingPosition, newBall);
        BallsList.Add(newBall);
      }

      foreach (var ball in BallsList)
      {
        ball.StartMoving();
      }
    }

    public override void LogData(string message)
    {
      _logger?.Log(LogLevel.Info, message);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!Disposed)
      {
        if (disposing)
        {
          foreach (var ball in BallsList)
          {
              ball.Dispose();
          }
          BallsList.Clear();

          _logger?.Dispose();
          _logger = null;
        }
        Disposed = true;
      }
      else
        throw new ObjectDisposedException(nameof(DataImplementation));
    }

    public override void MoveInteractiveBall(double x, double y)
    {
      _interactiveBall?.SetPosition(x, y);
    }

    public override void Dispose()
    {
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }

    private bool Disposed = false;
    private List<Ball> BallsList = [];

    [Conditional("DEBUG")]
    internal void CheckBallsList(Action<IEnumerable<IBall>> returnBallsList)
    {
      returnBallsList(BallsList);
    }

    [Conditional("DEBUG")]
    internal void CheckNumberOfBalls(Action<int> returnNumberOfBalls)
    {
      returnNumberOfBalls(BallsList.Count);
    }

    [Conditional("DEBUG")]
    internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
    {
      returnInstanceDisposed(Disposed);
    }
  }
}