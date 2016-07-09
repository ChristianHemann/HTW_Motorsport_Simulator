using System;
using CalculationComponents;
using ImportantClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Output;
using Simulator;

namespace UnitTests
{
    [TestClass]
    public class WheelTest
    {
        private Wheels _wheels;
        [TestInitialize]
        public void Init()
        {
            _wheels = new Wheels();

            SuspensionOutput sout0 = new SuspensionOutput();
            sout0.AccelerationTorque = 0;
            sout0.WheelAngle = (float)Math.PI / 4;
            sout0.WheelLoad = 50 * 9.81f;
            SuspensionOutput sout1 = new SuspensionOutput();
            sout1.AccelerationTorque = 0;
            sout1.WheelAngle = (float)Math.PI / 5;
            sout1.WheelLoad = 50 * 9.81f;
            SuspensionOutput sout2 = new SuspensionOutput();
            sout2.AccelerationTorque = 50;
            sout2.WheelAngle = 0;
            sout2.WheelLoad = 50 * 9.81f;
            SuspensionOutput sout3 = new SuspensionOutput();
            sout3.AccelerationTorque = 50;
            sout3.WheelAngle = 0;
            sout3.WheelLoad = 50 * 9.81f;
            SuspensionOutput.LastCalculations[0] = sout0;
            SuspensionOutput.LastCalculations[1] = sout1;
            SuspensionOutput.LastCalculations[2] = sout2;
            SuspensionOutput.LastCalculations[3] = sout3;

            SteeringOutput.LastCalculation.RadiusFrontAxis = 10;
            SteeringOutput.LastCalculation.RadiusRearAxis = 10;

            CalculationController.Instance.Wheels.Diameter = 0.5f;
            CalculationController.Instance.OverallCar.TrackWidth = 1.5f;
            CalculationController.Initialize(1);

            BrakeOutput.LastCalculation.BrakeMomentFront = 10;
            BrakeOutput.LastCalculation.BrakeMomentRear = 10;
            OverallCarOutput.LastCalculation.Speed = 10;
            OverallCarOutput.LastCalculation.Direction = new Vector3(1, 0, 0);
            
        }

        [TestMethod]
        public void TestWheels()
        {
            _wheels.Calculate();
            _wheels.StoreResult();
            Assert.IsTrue(new Vector2(1,0).Rotate(Math.PI / 4).Equals(WheelOutput.LastCalculations[0].Direction.Normalize(),1e-5f));
            Assert.IsTrue(new Vector2(1,0).Rotate(Math.PI / 5).Equals(WheelOutput.LastCalculations[1].Direction.Normalize(),1e-5f));
            Assert.IsTrue(new Vector2(1,0).Equals(WheelOutput.LastCalculations[2].Direction.Normalize(),1e-5f));
            Assert.IsTrue(new Vector2(1,0).Equals(WheelOutput.LastCalculations[3].Direction.Normalize(),1e-5f));

            Assert.AreEqual(-100/9.25, WheelOutput.LastCalculations[0].LateralAcceleration, 1e-5);
            Assert.AreEqual(-100/10.75, WheelOutput.LastCalculations[1].LateralAcceleration, 1e-5);
            Assert.AreEqual(0, WheelOutput.LastCalculations[2].LateralAcceleration);
            Assert.AreEqual(0, WheelOutput.LastCalculations[3].LateralAcceleration);

            Assert.AreEqual(0, WheelOutput.LastCalculations[0].Slip);
            Assert.AreEqual(0, WheelOutput.LastCalculations[1].Slip);
            Assert.AreEqual(0, WheelOutput.LastCalculations[2].Slip);
            Assert.AreEqual(0, WheelOutput.LastCalculations[3].Slip);

            Assert.AreEqual(0,WheelOutput.LastCalculations[0].LongitudinalAccelerationForce);
            Assert.AreEqual(0,WheelOutput.LastCalculations[1].LongitudinalAccelerationForce);
            Assert.AreEqual(200,WheelOutput.LastCalculations[2].LongitudinalAccelerationForce);
            Assert.AreEqual(200,WheelOutput.LastCalculations[3].LongitudinalAccelerationForce);

            Assert.AreEqual(5,WheelOutput.LastCalculations[0].LongitudinalDecelerationForce);
            Assert.AreEqual(5,WheelOutput.LastCalculations[1].LongitudinalDecelerationForce);
            Assert.AreEqual(5,WheelOutput.LastCalculations[2].LongitudinalDecelerationForce);
            Assert.AreEqual(5,WheelOutput.LastCalculations[3].LongitudinalDecelerationForce);
        }
    }
}
