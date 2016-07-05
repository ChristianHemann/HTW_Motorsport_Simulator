using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImportantClasses;
using Output;
using CalculationComponents;

namespace UnitTests
    
{
    [TestClass]
    public class EngineTest
    {
        [TestMethod]
        public void TestEngine()
        {
            Engine eg = new Engine();
            eg.MaxRpm = 5;
            // eg.CharacteristicLine = new Spline(new[] { 0.0, 10000.0 }, new[] { 0.0, 100.0 });
            InputData.UsedInputData = new InputData(0, 0, 0, 0);
            InputData.UsedInputData.AccelerationPedal = 0.5f;
            eg.Calculate();
            eg.StoreResult();
            Assert.AreEqual(0, EngineOutput.LastCalculation.Torque);
            Assert.AreEqual(0, EngineOutput.LastCalculation.Rpm);
        }
    }
}
