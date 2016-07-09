using System;
using CalculationComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Output;
using Simulator;
using Wheels = ImportantClasses.Enums.Wheels;

namespace UnitTests
{
    [TestClass]
    public class SuspensionTest
    {
        private Suspension _suspension;

        [TestInitialize]
        public void Init()
        {
            SteeringOutput.LastCalculation.WheelAngleLeft = (float)Math.PI / 4;
            SteeringOutput.LastCalculation.WheelAngleRight = (float)Math.PI / 5;
            CalculationController.Instance.OverallCar.Weight = 200;
            SecondaryDriveOutput.LastCalculation.Torque = 100;
            _suspension = new Suspension();
        }
        [TestMethod]
        public void TestSuspension()
        {
            _suspension.Calculate();
            _suspension.StoreResult();
            Assert.AreEqual(SuspensionOutput.GetLastCalculation(Wheels.FrontLeft).AccelerationTorque, 0);
            Assert.AreEqual(SuspensionOutput.GetLastCalculation(Wheels.FrontLeft).WheelAngle, Math.PI / 4, 1e-5);
            Assert.AreEqual(SuspensionOutput.GetLastCalculation(Wheels.FrontLeft).WheelLoad, 50 * 9.81f, 1e-5);
            Assert.AreEqual(SuspensionOutput.GetLastCalculation(Wheels.FrontRight).AccelerationTorque, 0);
            Assert.AreEqual(SuspensionOutput.GetLastCalculation(Wheels.FrontRight).WheelAngle, Math.PI / 5, 1e-5);
            Assert.AreEqual(SuspensionOutput.GetLastCalculation(Wheels.FrontRight).WheelLoad, 50 * 9.81f, 1e-5);

            Assert.AreEqual(SuspensionOutput.GetLastCalculation(Wheels.RearLeft).AccelerationTorque, 50);
            Assert.AreEqual(SuspensionOutput.GetLastCalculation(Wheels.RearLeft).WheelAngle, 0, 1e-5);
            Assert.AreEqual(SuspensionOutput.GetLastCalculation(Wheels.RearLeft).WheelLoad, 50 * 9.81f, 1e-5);
            Assert.AreEqual(SuspensionOutput.GetLastCalculation(Wheels.RearRight).AccelerationTorque, 50);
            Assert.AreEqual(SuspensionOutput.GetLastCalculation(Wheels.RearRight).WheelAngle, 0, 1e-5);
            Assert.AreEqual(SuspensionOutput.GetLastCalculation(Wheels.RearRight).WheelLoad, 50 * 9.81f, 1e-5);
        }
    }
}
