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
  public class EnvelopePoolTests
  {
    private class EnvelopeManagerFixture : IEnvelopeManager
    {
      public void ReturnEmptyEnvelope(IEnvelope envelope) {}
    }

    private class DTOFixture : Envelope
    {
      public DTOFixture(IEnvelopeManager manager) : base(manager) { }
    }

    [TestMethod]
    public void GetEmptyEnvelope_ShouldReturnNewInstance()
    {
      EnvelopePool<IEnvelope> pool = new EnvelopePool<IEnvelope>(source => new DTOFixture(source));

      IEnvelope result = pool.GetEmptyEnvelope();

      Assert.IsNotNull(result);
    }

    [TestMethod]
    public void ReturnEmptyEnvelope_ShouldAddBackToPool()
    {
      EnvelopePool<IEnvelope> pool = new EnvelopePool<IEnvelope>(source => new DTOFixture(source));
      IEnvelope envelope = pool.GetEmptyEnvelope();

      pool.ReturnEmptyEnvelope(envelope);

      IEnvelope secondResult = pool.GetEmptyEnvelope();
      Assert.IsNotNull(secondResult);
    }
  }
}