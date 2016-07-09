using CalculationComponents;
using ImportantClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Output;
using Simulator;

namespace UnitTests
{
    [TestClass]
    public class OverallCarTest
    {
        private OverallCar _overallCar;
        [TestInitialize]
        public void Init()
        {
            _overallCar = new OverallCar();
            _overallCar.Wheelbase = 1.5f;
            _overallCar.Weight = 200;
            _overallCar.TrackWidth = 1.5f;
            _overallCar.Drag = new Spline(new[] {0d, 100d}, new[] {0d, 0d});

            OverallCarOutput.LastCalculation.Direction = new Vector3(1, 0, 0);
            OverallCarOutput.LastCalculation.Speed = 10;
            OverallCarOutput.LastCalculation.Position = new Vector3();

            WheelOutput.LastCalculations[0] = new WheelOutput();
            WheelOutput.LastCalculations[0].Direction = new Vector2(1, 1).Normalize();
            WheelOutput.LastCalculations[0].LateralAcceleration = 100;
            WheelOutput.LastCalculations[0].LongitudinalAccelerationForce = 0;
            WheelOutput.LastCalculations[0].LongitudinalDecelerationForce = 10;
            WheelOutput.LastCalculations[0] = new WheelOutput();
            WheelOutput.LastCalculations[0].Direction = new Vector2(1, 1).Normalize();
            WheelOutput.LastCalculations[0].LateralAcceleration = 100;
            WheelOutput.LastCalculations[0].LongitudinalAccelerationForce = 0;
            WheelOutput.LastCalculations[0].LongitudinalDecelerationForce = 10;
            WheelOutput.LastCalculations[0] = new WheelOutput();
            WheelOutput.LastCalculations[0].Direction = new Vector2(1, 0).Normalize();
            WheelOutput.LastCalculations[0].LateralAcceleration = 0;
            WheelOutput.LastCalculations[0].LongitudinalAccelerationForce = 50;
            WheelOutput.LastCalculations[0].LongitudinalDecelerationForce = 10;
            WheelOutput.LastCalculations[0] = new WheelOutput();
            WheelOutput.LastCalculations[0].Direction = new Vector2(1, 0).Normalize();
            WheelOutput.LastCalculations[0].LateralAcceleration = 0;
            WheelOutput.LastCalculations[0].LongitudinalAccelerationForce = 50;
            WheelOutput.LastCalculations[0].LongitudinalDecelerationForce = 10;
            
            CalculationController.Initialize(1);
        }

        [TestMethod]
        public void TestOverallCar()
        {
            _overallCar.Calculate();
            _overallCar.StoreResult();

            Assert.IsTrue(OverallCarOutput.LastCalculation.Direction.Equals(new Vector3(1,0,0),1e-5f));
            Assert.AreEqual(10.2f, OverallCarOutput.LastCalculation.Speed);
            Assert.IsTrue(OverallCarOutput.LastCalculation.Position.Equals(new Vector3(10.2f,0,0),1e-5f));
        }
    }
}
