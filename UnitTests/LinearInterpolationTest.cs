using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImportantClasses;

namespace UnitTests
{
    [TestClass]
    public class LinearInterpolationTest
    {
        [TestMethod]
        public void TestGeneral()
        {
            LinearInterpolation interpolation = new LinearInterpolation(new [] {1.0,2.0,3.0},new []{1.0,2.0,3.0});
            double value = interpolation.Interpolate(1.5);
            Assert.AreEqual(1.5, value);
        }

        [TestMethod]
        public void TestUnsortedValues()
        {
            LinearInterpolation interpolation = new LinearInterpolation(new[] { 2.0, 1.0, 3.0, 1.5}, new[] { 2.0, 1.0, 3.0, 1.5});
            double value = interpolation.Interpolate(2.5);
            Assert.AreEqual(2.5, value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTwiceValues()
        {
            LinearInterpolation interpolation = new LinearInterpolation(new[] { 1.0, 2.0, 3.0, 3.0 }, new[] { 1.0, 2.0, 3.0, 3.0 });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestUnsortedTwiceValues()
        {
            LinearInterpolation interpolation = new LinearInterpolation(new[] {2.0, 1.0, 3.0, 3.0, 1.5}, new[] {2.0, 1.0, 3.0, 50.0, 1.5});
        }

        [TestMethod]
        public void TestValueOutOfRange()
        {
            LinearInterpolation interpolation = new LinearInterpolation(new[] { 1.0, 2.0, 3.0 }, new[] { 1.0, 2.0, 3.0 });
            double value = interpolation.Interpolate(3.5);
            Assert.AreEqual(3.5, value);
        }

        [TestMethod]
        public void TestWriteReadXml()
        {
            LinearInterpolation interpolation = new LinearInterpolation(new[] { 2.0, 1.0, 3.0, 1.5 }, new[] { 2.0, 1.0, 3.0, 1.5 });
            Xml.WriteXml(@"D:\Studium\SWE\bla.xml",interpolation);
            LinearInterpolation interpolation2 = Xml.ReadXml<LinearInterpolation>(@"D:\Studium\SWE\bla.xml");
            double value = interpolation2.Interpolate(2.5);
            Assert.AreEqual(2.5, value);
        }
    }
}
