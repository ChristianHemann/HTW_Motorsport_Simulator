using System;
using CalculationComponents;
using ImportantClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Output;
using Simulator;

namespace UnitTests
{
    [TestClass]
    public class SteeringTest
    {
        [TestMethod]
        public void TestSteering()
        {
            CalculationController.Instance.OverallCar.Wheelbase = 2;
            Steering steering = new Steering();
            steering.MaxSteeringAngle = (float)Math.PI;
            steering.LeftWheelAngle = new Spline(new[] { -Math.PI, Math.PI }, new[] { Math.PI / 4, -Math.PI / 4 });
            steering.RightWheelAngle = new Spline(new[] { -Math.PI, Math.PI }, new[] { Math.PI / 4, -Math.PI / 4 });
            InputData.UsedInputData = new InputData(0, 0, 0, 0);
            InputData.UsedInputData.Steering = 0.5f;
            steering.Calculate();
            steering.StoreResult();
            Assert.AreEqual(-Math.PI / 8, SteeringOutput.LastCalculation.WheelAngleLeft, 1e-6);
            Assert.AreEqual(-Math.PI / 8, SteeringOutput.LastCalculation.WheelAngleRight, 1e-6);
            Assert.AreEqual(5.22625, SteeringOutput.LastCalculation.RadiusFrontAxis,1e-5);
            Assert.AreEqual(4.82843, SteeringOutput.LastCalculation.RadiusRearAxis, 1e-5);
        }
    }
}
