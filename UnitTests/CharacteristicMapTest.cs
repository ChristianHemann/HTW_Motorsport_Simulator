using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImportantClasses;

namespace UnitTests
{
    [TestClass]
    public class CharacteristicMapTest
    {
        [TestMethod]
        public void TestGeneral()
        {
            double[] x = new[] {1.0, 2.0, 3.0};
            double[] y = new[] {1.0, 2.0, 3.0};
            double[][] values = new[]
            {
                new[] {1.0, 2.0, 3.0},
                new[] {2.0, 3.0, 4.0},
                new[] {3.0, 4.0, 5.0}
            };
            CharacteristicMap map = new CharacteristicMap(x, y, values);
            double value1 = map.Interpolate(1.5, 1.5); //should be 2.0
            Assert.AreEqual(value1, 2.0);
        }

        [TestMethod]
        public void TestWriteReadXml()
        {
            string path = Path.GetTempFileName();
            double[] x = new[] { 1.0, 2.0, 3.0 };
            double[] y = new[] { 1.0, 2.0, 3.0 };
            double[][] values = new[]
            {
                new[] {1.0, 2.0, 3.0},
                new[] {2.0, 3.0, 4.0},
                new[] {3.0, 4.0, 5.0}
            };
            CharacteristicMap map = new CharacteristicMap(x, y, values);
            Xml.WriteXml(path,map);
            CharacteristicMap map2 = Xml.ReadXml<CharacteristicMap>(path);
            Assert.AreEqual(2.0,map2.Interpolate(1.5, 1.5));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTwiceValues()
        {
            double[] x = new[] { 1.0, 2.0, 3.0 };
            double[] y = new[] { 1.0, 1.0, 3.0 };
            double[][] values = new[]
            {
                new[] {1.0, 2.0, 3.0},
                new[] {2.0, 3.0, 4.0},
                new[] {3.0, 4.0, 5.0}
            };
            CharacteristicMap map = new CharacteristicMap(x, y, values);
        }

        [TestMethod]
        public void TestUnsortedValues()
        {
            double[] x = new[] { 3.0, 1.0, 2.0 };
            double[] y = new[] { 2.0, 3.0, 1.0 };
            double[][] values = new[]
            {
                new[] {4.0, 5.0, 3.0},
                new[] {2.0, 3.0, 1.0},
                new[] {3.0, 4.0, 2.0}
            };
            CharacteristicMap map = new CharacteristicMap(x, y, values);
            double value1 = map.Interpolate(1.5, 1.5); //should be 2.0
            Assert.AreEqual(value1, 2.0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSmallValue()
        {
            double[] x = new[] { 1.0, 2.0, 3.0 };
            double[] y = new[] { 1.0, 2.0, 3.0 };
            double[][] values = new[]
            {
                new[] {1.0, 2.0, 3.0},
                new[] {2.0, 3.0, 4.0},
                new[] {3.0, 4.0, 5.0}
            };
            CharacteristicMap map = new CharacteristicMap(x, y, values);
            try
            {
                double value1 = map.Interpolate(0.5, 1.5);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                try
                {
                    double value2 = map.Interpolate(0.5, 0.5);
                }
                catch (ArgumentOutOfRangeException ex2)
                {
                    double value3 = map.Interpolate(1.5, 0.5); //here should the 3rd exception be thrown
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestBigValues()
        {
            double[] x = new[] { 1.0, 2.0, 3.0 };
            double[] y = new[] { 1.0, 2.0, 3.0 };
            double[][] values = new[]
            {
                new[] {1.0, 2.0, 3.0},
                new[] {2.0, 3.0, 4.0},
                new[] {3.0, 4.0, 5.0}
            };
            CharacteristicMap map = new CharacteristicMap(x, y, values);
            try
            {
                double value1 = map.Interpolate(1.5, 3.5);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                try
                {
                    double value2 = map.Interpolate(3.5, 1.5);
                }
                catch (ArgumentOutOfRangeException ex2)
                {
                    double value3 = map.Interpolate(3.5, 3.5); //here should the 3rd exception be thrown
                }
            }
        }
    }
}
