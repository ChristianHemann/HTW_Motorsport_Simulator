using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Output;

namespace UnitTests
{
    [TestClass]
    public class GearBoxTest
    {
        private GearBox _gearBox;
        private float[] _expectedValues;

        [TestInitialize]
        public void Init()
        {
            _gearBox = new GearBox();
            _gearBox.Efficency = 0.9f;
            _gearBox.Gears = 3;
            _gearBox.Gear = 0;
            _gearBox.Transmissions[0] = 10;
            _gearBox.Transmissions[1] = 5;
            _gearBox.Transmissions[2] = 1;
            EngineOutput.LastCalculation = new EngineOutput();
            EngineOutput.LastCalculation.torque = 100;
            _expectedValues = new[] {0, 900f, 450f, 90f};
        }

        [TestMethod]
        public void TestGear()
        {
            for (int i = 0; i < _expectedValues.Length; i++)
            {
                _gearBox.Gear = (sbyte)i;
                _gearBox.Calculate();
                _gearBox.StoreResult();
                Assert.AreEqual(GearBoxOutput.LastCalculation.Torque, _expectedValues[i], 1e-6);
            }
        }
    }
}
