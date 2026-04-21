//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using System;
using System.Diagnostics;

namespace TP.ConcurrentProgramming.Data
{
  internal class DataImplementation : DataAbstractAPI
  {
    internal const double BoardWidth = 400.0;
    internal const double BoardHeight = 400.0;
    internal const double BallRadius = 10.0;

    public DataImplementation()
    {
      MoveTimer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(30));
    }

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
        
        Ball newBall = new(startingPosition, startingVelocity);
        upperLayerHandler(startingPosition, newBall);
        BallsList.Add(newBall);
      }
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!Disposed)
      {
        if (disposing)
        {
          MoveTimer.Dispose();
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
    private readonly Timer MoveTimer;
    private List<Ball> BallsList = [];

    private void Move(object? x)
    {
      foreach (Ball item in BallsList)
        item.Move(BoardWidth, BoardHeight, BallRadius);
    }

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