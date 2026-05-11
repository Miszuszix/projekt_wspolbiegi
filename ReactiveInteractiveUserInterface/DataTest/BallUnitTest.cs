//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

namespace TP.ConcurrentProgramming.Data.Test
{
  [TestClass]
  public class BallUnitTest
  {
    [TestMethod]
    public void ConstructorTestMethod()
    {
      Vector testingVector = new Vector(0.0, 0.0);
      
      Ball newInstance = new(testingVector, testingVector, 100.0, 100.0, 5.0);
      Assert.IsNotNull(newInstance);
    }

    [TestMethod]
    public void MoveTestMethod()
    {
      Vector initialPosition = new(10.0, 10.0);
      Vector initialVelocity = new(5.0, 5.0);
      
      Ball newInstance = new(initialPosition, initialVelocity, 100.0, 100.0, 5.0);
      
      IVector curentPosition = new Vector(0.0, 0.0);
      int numberOfCallBackCalled = 0;
      
      newInstance.NewPositionNotification += (sender, position) => 
      { 
        Assert.IsNotNull(sender); 
        curentPosition = position; 
        numberOfCallBackCalled++; 
      };
      
      newInstance.Move();
      
      Assert.AreEqual<int>(1, numberOfCallBackCalled);
      Assert.AreEqual<double>(15.0, curentPosition.x);
      Assert.AreEqual<double>(15.0, curentPosition.y);
    }

    [TestMethod]
    public void Move_BounceOffWallTestMethod()
    {
      Vector initialPosition = new(96.0, 10.0);
      Vector initialVelocity = new(5.0, 0.0); // Leci w prawo
      
      Ball newInstance = new(initialPosition, initialVelocity, 100.0, 100.0, 5.0);
      
      newInstance.Move();
      
      Assert.AreEqual<double>(-5.0, newInstance.Velocity.x);
    }
  }
}