using System;
using CalculationComponents.TrackComponents;
using ImportantClasses;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class StraightTest
    {
        private static Vector2 p0 = new Vector2();
        private static Vector2 p1 = new Vector2(0, 1);
        private static Vector2 v1 = new Vector2(1, 0);
        private static Vector2 v2 = new Vector2(1, 1);
        private static Vector2 v3 = new Vector2(0, 1);
        private static StartLine previous = new StartLine(null, 5f, p0, v1);

        [TestMethod]
        public void TestStraightDerivedValues()
        {
            Straight straight = new Straight(previous, 5f, 1f);
            straight.EndPoint = new Vector2(20,0);
            Assert.AreEqual(20f, straight.Length, 1E-10);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestStraightException()
        {
            Straight straight = new Straight(previous, 5f, 1f);
            straight.EndPoint = new Vector2(10,20);
        }

        [TestMethod]
        public void TestStraightBaseValues()
        {
            Straight straight = new Straight(previous, 5f, 1f);
            Assert.IsTrue(straight.EndPoint.Equals(v1, (float)1E-5));
        }

        [TestMethod]
        public void TestStraightPreviousTrackSegmentChanged()
        {
            Straight straight = new Straight(previous, 5f, 1f);
            previous.TrySetEndDirection(v3);
            Assert.IsTrue(straight.EndPoint.Equals(v3, (float) 1E-5));
            previous.TrySetEndDirection(v1); //make sure the old value is restored for other tests
        }
    }
}
