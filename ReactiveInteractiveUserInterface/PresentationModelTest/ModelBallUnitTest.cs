//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using TP.ConcurrentProgramming.Presentation.Model;
using TP.ConcurrentProgramming.BusinessLogic;
using LogicIBall = TP.ConcurrentProgramming.BusinessLogic.IBall;

namespace TP.ConcurrentProgramming.Presentation.Model.Test
{
    [TestClass]
    public class ModelBallUnitTest
    {
        [TestMethod]
        public void Constructor_ShouldSetCorrectInitialPosition()
        {
            var logicBall = new BusinessLogicIBallFixture();
            ModelBall ball = new ModelBall(50.0, 50.0, 10.0, logicBall);

            Assert.AreEqual(40.0, ball.Left);
            Assert.AreEqual(40.0, ball.Top);
        }

        [TestMethod]
        public void PositionChange_ShouldUpdateModel_WhenLogicNotifies()
        {
            int notificationCounter = 0;
            var logicBall = new BusinessLogicIBallFixture();
            ModelBall ball = new ModelBall(50.0, 50.0, 10.0, logicBall);
            
            ball.PropertyChanged += (sender, args) => notificationCounter++;

            logicBall.SimulateMove(60.0, 60.0);
            
            Assert.IsTrue(notificationCounter > 0);
            Assert.AreEqual(50.0, ball.Left);
            Assert.AreEqual(50.0, ball.Top);
        }

        #region testing instrumentation (Ręczne atrapy zamiast Moq)

        private class BusinessLogicIBallFixture : LogicIBall
        {
            public double x { get; set; }
            public double y { get; set; }
            public double radius { get; set; } = 10;
            public double weight => 0;
            public double xSpeed => 0;
            public double ySpeed => 0;

            public event EventHandler<IPosition>? NewPositionNotification;

            public void SimulateMove(double newX, double newY)
            {
                x = newX;
                y = newY;
                NewPositionNotification?.Invoke(this, new PositionFixture(newX, newY));
            }

            public void Move() { }
            public void Dispose() { }
        }
        private class PositionFixture : IPosition
        {
            public double x { get; init; }
            public double y { get; init; }

            public PositionFixture(double x, double y) 
            { 
                this.x = x; 
                this.y = y; 
            }
        }

        #endregion
    }
}