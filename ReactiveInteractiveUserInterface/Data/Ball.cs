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
  internal class Ball : IBall
  {
    internal Ball(Vector initialPosition, Vector initialVelocity)
    {
      Position = initialPosition;
      Velocity = initialVelocity;
    }

    public event EventHandler<IVector>? NewPositionNotification;

    public IVector Velocity { get; set; }

    private Vector Position;

    private void RaiseNewPositionChangeNotification()
    {
      NewPositionNotification?.Invoke(this, Position);
    }

    internal void Move(double boardWidth, double boardHeight, double radius)
    {
      double newX = Position.x + Velocity.x;
      double newY = Position.y + Velocity.y;
      double newVX = Velocity.x;
      double newVY = Velocity.y;

      if (newX <= radius || newX >= boardWidth - radius)
      {
        newVX = -newVX;
        newX = newX <= radius ? radius : boardWidth - radius;
      }
      
      if (newY <= radius || newY >= boardHeight - radius)
      {
        newVY = -newVY;
        newY = newY <= radius ? radius : boardHeight - radius;
      }

      Velocity = new Vector(newVX, newVY);
      Position = new Vector(newX, newY);
      RaiseNewPositionChangeNotification();
    }
  }
}