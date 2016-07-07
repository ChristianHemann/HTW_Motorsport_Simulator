using System;
using CalculationComponents;
using ImportantClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Output;
using Simulator;

namespace UnitTests
{
    [TestClass]
    public class SecondaryDriveTest
    {
        private SecondaryDrive secondaryDrive;
        [TestInitialize]
        public void Init()
        {
            secondaryDrive = new SecondaryDrive();
            secondaryDrive.Transmission = 2; // rpmIn/prmOut
            GearBoxOutput.LastCalculation.Torque = 100; // 1/min
            OverallCarOutput.LastCalculation.Speed = 20f; // m/s
            CalculationController.Instance.Wheels.Diameter = 0.3f; //m
        }

        [TestMethod]
        public void TestSecondaryCalculate()
        {
            secondaryDrive.Calculate();
            secondaryDrive.StoreResult();
            Assert.AreEqual(200,SecondaryDriveOutput.LastCalculation.Torque,1e-5);
        }

        [TestMethod]
        public void TestSecondaryCalculateBackwards()
        {
            secondaryDrive.CalculateBackwards();
            secondaryDrive.StoreResult();
            Assert.AreEqual(1273.2395, SecondaryDriveOutput.LastCalculation.Rpm, 1e-2);
        }
    }
}
