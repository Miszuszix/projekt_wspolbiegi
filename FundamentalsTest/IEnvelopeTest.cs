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

namespace TP.ConcurrentProgramming.Fundamentals.Test
{
  [TestClass]
  public class IEnvelopeTest
  {
    private class EnvelopeFixture : IEnvelope
    {
      public bool InPool { get; set; } = false;
      public IEnvelopeManager GetIEnvelopeManager => null!;
      public void ReturnEmptyEnvelope() { }
    }

    [TestMethod]
    public void EnvelopeFixture_PropertyTest()
    {
      IEnvelope envelope = new EnvelopeFixture { InPool = true };
      Assert.IsTrue(envelope.InPool);
    }
  }
}