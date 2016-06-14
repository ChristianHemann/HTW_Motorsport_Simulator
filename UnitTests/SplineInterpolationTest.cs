using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImportantClasses;

namespace UnitTests
{
    [TestClass]
    public class SplineInterpolationTest
    {

        [TestMethod]
        public void TestGeneral()
        {
            Spline interpolation = new Spline(new[] { 1.0, 2.0, 3.0 }, new[] { 1.0, 4.0, 9.0 });
            double value = interpolation.Interpolate(1.5);
            Assert.AreEqual(2.3125, value);
        }

        [TestMethod]
        public void TestUnsortedValues()
        {
            Spline interpolation = new Spline(new[] { 2.0, 1.0, 3.0, 1.5 }, new[] { 2.0, 1.0, 3.0, 1.5 });
            double value = interpolation.Interpolate(2.5);
            Assert.AreEqual(2.5, value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTwiceValues()
        {
            Spline interpolation = new Spline(new[] { 1.0, 2.0, 3.0, 3.0 }, new[] { 1.0, 2.0, 3.0, 3.0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUnsortedTwiceValues()
        {
            Spline interpolation = new Spline(new[] { 2.0, 1.0, 3.0, 3.0, 1.5 }, new[] { 2.0, 1.0, 3.0, 50.0, 1.5 });
        }

        [TestMethod]
        public void TestValueOutOfRange()
        {
            Spline interpolation = new Spline(new[] { 1.0, 2.0, 3.0 }, new[] { 1.0, 2.0, 3.0 });
            double value = interpolation.Interpolate(3.5);
            Assert.AreEqual(3.5, value);
        }

        [TestMethod]
        public void TestWriteReadXml()
        {
            string filePath = Path.GetTempFileName();
            Spline interpolation = new Spline(new[] { 2.0, 1.0, 3.0, 1.5 }, new[] { 2.0, 1.0, 3.0, 1.5 });
            Xml.WriteXml(filePath, interpolation);
            Spline interpolation2 = Xml.ReadXml<Spline>(filePath);
            double value = interpolation2.Interpolate(2.5);
            Assert.AreEqual(2.5, value);
        }
    }
}
