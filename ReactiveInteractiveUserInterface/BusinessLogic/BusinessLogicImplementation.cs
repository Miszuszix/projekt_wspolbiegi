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
using UnderneathLayerAPI = TP.ConcurrentProgramming.Data.DataAbstractAPI;
using DataBall = TP.ConcurrentProgramming.Data.IBall;
using DataVector = TP.ConcurrentProgramming.Data.IVector;

namespace TP.ConcurrentProgramming.BusinessLogic
{
  internal class VectorWrapper : DataVector
  {
    public double x { get; init; }
    public double y { get; init; }
    
    public VectorWrapper(double x, double y)
    {
      this.x = x;
      this.y = y;
    }
  }

  internal class BusinessLogicImplementation : BusinessLogicAbstractAPI
  {
    private bool Disposed = false;
    private readonly UnderneathLayerAPI layerBellow;
    private readonly List<DataBall> _balls = new List<DataBall>();

    public BusinessLogicImplementation() : this(null) { }

    internal BusinessLogicImplementation(UnderneathLayerAPI? underneathLayer)
    {
      layerBellow = underneathLayer == null ? UnderneathLayerAPI.GetDataLayer() : underneathLayer;
    }

    public override void Dispose()
    {
      if (Disposed) throw new ObjectDisposedException(nameof(BusinessLogicImplementation));
      layerBellow.Dispose();
      Disposed = true;
    }

    public override void Start(int numberOfBalls, Action<IPosition, IBall> upperLayerHandler)
    {
      if (Disposed) throw new ObjectDisposedException(nameof(BusinessLogicImplementation));
      if (upperLayerHandler == null) throw new ArgumentNullException(nameof(upperLayerHandler));

      _balls.Clear();

      layerBellow.Start(numberOfBalls, (startingPosition, databall) =>
      {
        _balls.Add(databall);
        
        databall.NewPositionNotification += (sender, newPosition) => DetectCollisions(databall, newPosition);

        upperLayerHandler(new Position(startingPosition.x, startingPosition.y), new Ball(databall));
      });
    }

    private void DetectCollisions(DataBall currentBall, DataVector currentPos)
    {
      foreach (var otherBall in _balls)
      {
        if (currentBall == otherBall) continue;

        DataVector otherPos = otherBall.Position;

        double dx = currentPos.x - otherPos.x;
        double dy = currentPos.y - otherPos.y;
        
        double distanceSquared = dx * dx + dy * dy;
        
        if (distanceSquared < 0.0001) continue;
        double distance = Math.Sqrt(distanceSquared);

        if (distance <= 20)
        {
          DataVector v1 = currentBall.Velocity;
          DataVector v2 = otherBall.Velocity;

          double vx = v1.x - v2.x;
          double vy = v1.y - v2.y;

          double dotProduct = (dx * vx) + (dy * vy);

          if (dotProduct < 0)
          {
            double collisionScale = dotProduct / distanceSquared;

            double newVx1 = v1.x - collisionScale * dx;
            double newVy1 = v1.y - collisionScale * dy;

            double newVx2 = v2.x - collisionScale * -dx;
            double newVy2 = v2.y - collisionScale * -dy;

            currentBall.Velocity = new VectorWrapper(newVx1, newVy1);
            otherBall.Velocity = new VectorWrapper(newVx2, newVy2);
          }
        }
      }
    }

    [Conditional("DEBUG")]
    internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
    {
      returnInstanceDisposed(Disposed);
    }
  }
}