using System;
using CalculationComponents.TrackComponents;
using ImportantClasses;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CurveTest
    {
        private static Vector2 p0 = new Vector2();
        private static Vector2 p1 = new Vector2(0,1);
        private static Vector2 v1 = new Vector2(1,0);
        private static Vector2 v2 = new Vector2(1,1);
        private static Vector2 v3 = new Vector2(0,1);
        private static StartLine previous = new StartLine(null, 5f, p0, v1);

        [TestMethod]
        public void TestCurveRadius()
        {
            Curve curve = new Curve(previous, 5f, v2);
            Assert.AreEqual(1f,curve.Radius);
        }

        [TestMethod]
        public void TestCurveMiddlePoint()
        {
            Curve curve = new Curve(previous, 5f, v2);
            Assert.AreEqual(p1,curve.MiddlePoint);
        }

        [TestMethod]
        public void TestCurveAngle()
        {
            Curve curve = new Curve(previous, 5f, v2);
            Assert.AreEqual(Convert.ToSingle(Math.PI/2),curve.Angle);
        }

        [TestMethod]
        public void TestGetPoint()
        {
            Curve curve = new Curve(previous, 5f, v2);
            Assert.IsTrue(new Vector2(
            Convert.ToSingle(Math.Cos(Math.PI/4)), Convert.ToSingle(Math.Sin(Math.PI*2*7/8) + 1f))
                .Equals(curve.GetPointAtAngle(Convert.ToSingle(Math.PI/4)), (float) 1E-5));
        }

        [TestMethod]
        public void TestCurveChangeAngle()
        {
            Curve curve = new Curve(previous, 5f, v2);
            curve.Angle = (float) Math.PI;
            Assert.IsTrue(new Vector2(0,2).Equals(curve.EndPoint,(float)1E-5));
        }

        [TestMethod]
        public void TestCurveChangeRadius()
        {
            Curve curve = new Curve(previous, 5f, v2);
            curve.Radius = 10;
            Assert.IsTrue(curve.EndPoint.Equals(new Vector2(10,10), (float)1E-5));
        }

        [TestMethod]
        public void TestCurveEndDirection()
        {
            Curve curve = new Curve(previous, 5f, v2);
            Assert.IsTrue(v3.Equals(curve.EndDirection, (float) 1E-5));
        }

        [TestMethod]
        public void TestCurvePreviousTrackSegmentChanged()
        {
            Curve curve = new Curve(previous, 5f, v2);
            previous.TrySetEndDirection(v3);
            previous.EndPoint = p1;
            Assert.IsTrue(curve.EndPoint.Equals(new Vector2(-1,2),(float)1E-5));
            previous.TrySetEndDirection(v1);
        }
    }
}
