using System;
using CalculationComponents.TrackComponents;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CurveTest
    {
        private static Vector<float> p0 = Vector<float>.Build.Dense(2);
        private static Vector<float> p1 = Vector<float>.Build.DenseOfArray(new[] {0, 1f});
        private static Vector<float> v1 = Vector<float>.Build.DenseOfArray(new[] { 1f, 0f });
        private static Vector<float> v2 = Vector<float>.Build.DenseOfArray(new[] { 1f, 1f });
        private static Vector<float> v3 = Vector<float>.Build.DenseOfArray(new[] { 0f, 1f });
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
            Assert.AreEqual(
                Vector<float>.Build.DenseOfArray(new[]
                {Convert.ToSingle(Math.Cos(Math.PI/4)), Convert.ToSingle(Math.Sin(Math.PI*2*7/8) + 1f)}),
                curve.GetPointAtAngle(Convert.ToSingle(Math.PI/4)));
        }

        [TestMethod]
        public void TestCurveChangeAngle()
        {
            Curve curve = new Curve(previous, 5f, v2);
            Assert.AreEqual(v2,curve.EndPoint);
            curve.Angle = (float) Math.PI;
            Assert.AreEqual(Vector<float>.Build.DenseOfArray(new []{0,2f}),curve.EndPoint);
        }

        [TestMethod]
        public void TestCurveEndDirection()
        {
            Curve curve = new Curve(previous, 5f, v2);
            Assert.AreEqual(v3,curve.EndDirection);
        }
    }
}
