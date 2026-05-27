//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TP.ConcurrentProgramming.BusinessLogic;
using TP.ConcurrentProgramming.Data;

namespace TP.ConcurrentProgramming.BusinessLogic.Test
{
  internal class MockVector : Data.IVector
  {
    public double x { get; init; }
    public double y { get; init; }
    
    public MockVector(double x, double y)
    {
      this.x = x;
      this.y = y;
    }
  }

  internal class MockBall : Data.IBall
  {
    public event EventHandler<Data.IVector>? NewPositionNotification;
    public Data.IVector Velocity { get; set; }
    public Data.IVector Position { get; set; }

    public object BallLock { get; } = new object();

    public MockBall(double x, double y, double vx, double vy)
    {
      Position = new MockVector(x, y);
      Velocity = new MockVector(vx, vy);
    }

    public void SimulateMovement(double newX, double newY)
    {
      Position = new MockVector(newX, newY);
      NewPositionNotification?.Invoke(this, Position);
    }

    public void Dispose() { }
  }

  internal class MockDataAPI : DataAbstractAPI
  {
    public List<MockBall> Balls = new List<MockBall>();

    public override void LogData(string message)
    {
    }
    public override void MoveInteractiveBall(double x, double y)
    {
    }

    public override void Start(int numberOfBalls, Action<Data.IVector, Data.IBall> upperLayerHandler)
    {
      foreach (var ball in Balls)
      {
        upperLayerHandler(ball.Position, ball);
      }
    }

    public override void Dispose() { }
  }

  [TestClass]
  public class BusinessLogicUnitTest
  {
    [TestMethod]
    public void DetectCollisions_StandardCollisionTest()
    {
      var mockApi = new MockDataAPI();
      var ball1 = new MockBall(10, 10, 5, 0);
      var ball2 = new MockBall(30, 10, -5, 0);
      mockApi.Balls.Add(ball1);
      mockApi.Balls.Add(ball2);

      var logicApi = new BusinessLogicImplementation(mockApi);
      logicApi.Start(2, (pos, ball) => { });

      ball1.SimulateMovement(15, 10);
      ball2.SimulateMovement(25, 10);

      Assert.IsTrue(ball1.Velocity.x < 0);
      Assert.IsTrue(ball2.Velocity.x > 0);
    }

    [TestMethod]
    public void DetectCollisions_GhostCollisionTest()
    {
      var mockApi = new MockDataAPI();
      var ball1 = new MockBall(15, 10, -5, 0);
      var ball2 = new MockBall(25, 10, 5, 0);
      mockApi.Balls.Add(ball1);
      mockApi.Balls.Add(ball2);

      var logicApi = new BusinessLogicImplementation(mockApi);
      logicApi.Start(2, (pos, ball) => { });

      ball1.SimulateMovement(15, 10);

      Assert.AreEqual(-5, ball1.Velocity.x);
      Assert.AreEqual(5, ball2.Velocity.x);
    }

    [TestMethod]
    public void DetectCollisions_ThreeBallsCollisionTest()
    {
      var mockApi = new MockDataAPI();
      var ball1 = new MockBall(20, 20, 5, 5);
      var ball2 = new MockBall(30, 20, -5, 5);
      var ball3 = new MockBall(20, 30, 5, -5);
      mockApi.Balls.Add(ball1);
      mockApi.Balls.Add(ball2);
      mockApi.Balls.Add(ball3);

      var logicApi = new BusinessLogicImplementation(mockApi);
      logicApi.Start(3, (pos, ball) => { });

      ball1.SimulateMovement(20, 20);

      Assert.AreNotEqual(5, ball1.Velocity.x);
      Assert.AreNotEqual(5, ball1.Velocity.y);
    }
  }
}