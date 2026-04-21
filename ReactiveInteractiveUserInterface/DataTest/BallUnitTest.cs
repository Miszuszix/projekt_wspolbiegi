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
      Vector testinVector = new Vector(0.0, 0.0);
      Ball newInstance = new(testinVector, testinVector);
    }

    [TestMethod]
    public void MoveTestMethod()
    {
      Vector initialPosition = new(10.0, 10.0);
      Vector initialVelocity = new(5.0, 5.0); // Kula porusza się o 5 w osi X i Y
      Ball newInstance = new(initialPosition, initialVelocity);
      IVector curentPosition = new Vector(0.0, 0.0);
      int numberOfCallBackCalled = 0;
      newInstance.NewPositionNotification += (sender, position) => { Assert.IsNotNull(sender); curentPosition = position; numberOfCallBackCalled++; };
      
      newInstance.Move(100.0, 100.0, 5.0);
      
      Assert.AreEqual<int>(1, numberOfCallBackCalled);
      Assert.AreEqual<double>(15.0, curentPosition.x);
      Assert.AreEqual<double>(15.0, curentPosition.y);
    }
  }
}