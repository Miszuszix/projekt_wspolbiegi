using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TP.ConcurrentProgramming.Data.Test
{
  [TestClass]
  public class TimeAndSchedulingUnitTest
  {
    [TestMethod]
    public async Task MovementSchedulingFairnessTest()
    {
      using (DataImplementation api = new DataImplementation())
      {
        ConcurrentDictionary<IBall, int> moveCounts = new();

        api.Start(5, (pos, ball) =>
        {
          moveCounts[ball] = 0;
          ball.NewPositionNotification += (sender, newPos) =>
          {
            moveCounts.AddOrUpdate(ball, 1, (k, v) => v + 1);
          };
        });

        await Task.Delay(2000);

        int minMoves = int.MaxValue;
        int maxMoves = 0;

        foreach (var count in moveCounts.Values)
        {
          if (count < minMoves) minMoves = count;
          if (count > maxMoves) maxMoves = count;

          Assert.IsTrue(count > 0);
        }

        Assert.IsTrue(maxMoves - minMoves < maxMoves * 0.5);
      }
    }

    [TestMethod]
    public async Task HighResolutionTimerMeasurementTest()
    {
      Assert.IsTrue(Stopwatch.IsHighResolution);

      Vector initialPosition = new(10.0, 10.0);
      Vector initialVelocity = new(5.0, 5.0);
      Ball ball = new(initialPosition, initialVelocity, 100.0, 100.0, 5.0);

      Stopwatch stopwatch = new Stopwatch();
      int moves = 0;
      TaskCompletionSource tcs = new TaskCompletionSource();

      ball.NewPositionNotification += (sender, pos) =>
      {
        moves++;
        if (moves == 1)
        {
           stopwatch.Start();
        }
        else if (moves == 51)
        {
           stopwatch.Stop();
           tcs.SetResult();
        }
      };

      ball.StartMoving();

      await tcs.Task;
      ball.Dispose();

      long elapsed = stopwatch.ElapsedMilliseconds;

      Assert.IsTrue(elapsed >= 450 && elapsed < 1500);
    }

    [TestMethod]
    public async Task Performance_TenSecondsThousandMovesTest()
    {
      Vector initialPosition = new(10.0, 10.0);
      Vector initialVelocity = new(5.0, 5.0);
      Ball ball = new(initialPosition, initialVelocity, 100.0, 100.0, 5.0);

      int movesCount = 0;
      ball.NewPositionNotification += (sender, pos) =>
      {
        movesCount++;
      };

      ball.StartMoving();

      await Task.Delay(10000);

      ball.Dispose();

      Assert.IsTrue(movesCount >= 800 && movesCount <= 1100);
    }
  }
}