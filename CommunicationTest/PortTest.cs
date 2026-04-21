//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2025, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________

using TP.ConcurrentProgramming.Fundamentals;

namespace TP.ConcurrentProgramming.Communication.Test
{
  [TestClass]
  public class PortTest
  {
    private class EnvelopeFixture : IEnvelope
    {
      public bool InPool => false;
      public IEnvelopeManager GetIEnvelopeManager => null!;
      public void ReturnEmptyEnvelope() { }
    }

    [TestMethod]
    public void Open_ShouldSetOpenedToTrue()
    {
      using (Port _port = new Port())
      {
        _port.Open();
        Assert.IsTrue(_port.Count == 0);
      }
    }

    [TestMethod]
    public void SendMsg_ShouldAddMessageToQueue()
    {
      IEnvelope _envelope = new EnvelopeFixture();
      using (Port _port = new Port())
      {
        _port.Open();
        _port.SendMsg(ref _envelope!);
        Assert.AreEqual(1, _port.Count);
        Assert.IsNull(_envelope);
      }
    }
  }
}