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
    
    public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler)
    {
      if (Disposed)
        throw new ObjectDisposedException(nameof(DataImplementation));
      if (upperLayerHandler == null)
        throw new ArgumentNullException(nameof(upperLayerHandler));
        
      BallsList.Clear();
      Random random = new Random();
      
      for (int i = 0; i < numberOfBalls; i++)
      {
        Vector startingPosition = new(
            random.Next((int)BallRadius, (int)(BoardWidth - BallRadius)), 
            random.Next((int)BallRadius, (int)(BoardHeight - BallRadius))
        );
        Vector startingVelocity = new((random.NextDouble() - 0.5) * 10, (random.NextDouble() - 0.5) * 10);
        
        Ball newBall = new(startingPosition, startingVelocity, BoardWidth, BoardHeight, BallRadius);
        upperLayerHandler(startingPosition, newBall);
        BallsList.Add(newBall);
        
        newBall.StartMoving();
      }
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
        }
        Disposed = true;
      }
      else
        throw new ObjectDisposedException(nameof(DataImplementation));
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