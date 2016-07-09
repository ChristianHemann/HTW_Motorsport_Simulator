using System.Threading;
using ImportantClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Output;
using Simulator;

namespace UnitTests
{
    [TestClass]
    public class CalculationControllerTest
    {
        [TestInitialize]
        public void Init()
        {
            CalculationController.Instance.Brake.BrakeBalance = new MathHelper.Ratio(1, 1);
            CalculationController.Instance.Brake.NormalBrakeMoment = 100;
            InputData.UsedInputData = new InputData(0, 0, 0, 0);
        }

        [TestMethod]
        public void TestCalculationController()
        {
            InputData.ActualInputData = new InputData(0, 1, 0, 1);
            CalculationController.Initialize(1);
            CalculationController.Calculate();
            Thread.Sleep(100); //wait until the calculation should be ready
            Assert.AreNotEqual(BrakeOutput.LastCalculation.BrakeMomentFront, 0); //due to the calculate the value dshould have changed
            float oldValue = BrakeOutput.LastCalculation.BrakeMomentFront;
            InputData.ActualInputData = new InputData(1, 0, 0, 2);
            CalculationController.Interrupt();
            CalculationController.Calculate();
            Thread.Sleep(100); //wait until the calculation should be ready
            Assert.AreEqual(oldValue, BrakeOutput.LastCalculation.BrakeMomentFront); //the value should not have change because the calculation was interrupted
            CalculationController.Initialize(1); //Continue the Calculation
            CalculationController.Calculate();
            Thread.Sleep(100); //wait until the calculation should be ready
            Assert.AreEqual(0, BrakeOutput.LastCalculation.BrakeMomentFront);
            CalculationController.Terminate();
        }

        [TestMethod]
        [ExpectedException(typeof(ThreadStateException))]
        public void TestCalculationControllerTerminate()
        {
            CalculationController.Initialize(1);
            CalculationController.Terminate();
            CalculationController.Initialize(1); //an exception should be thrown because the workerThread was already started
        }
    }
}
