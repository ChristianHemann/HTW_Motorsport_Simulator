using System;
using ImportantClasses;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class VectorExtensionTest
    {
        [TestMethod]
        public void TestVectorMagnitude()
        {
            Vector<float> vector = Vector<float>.Build.DenseOfArray(new[] {3f, 4f});
            Vector<float> v1 = Vector<float>.Build.DenseOfArray(new[] { 0, 1f });
            Vector<float> v2 = Vector<float>.Build.DenseOfArray(new[] { 1f, 1f });
            Vector<float> v3 = Vector<float>.Build.DenseOfArray(new[] { 1f, 0f });
            Assert.AreEqual(5,vector.GetMagnitude());
            Assert.AreEqual(1f, v1.GetMagnitude());
            Assert.AreEqual(Convert.ToSingle(Math.Sqrt(2)), v2.GetMagnitude());
            Assert.AreEqual(1f,v3.GetMagnitude());
        }

        [TestMethod]
        public void TestVectorNormal()
        {
            Vector<float> vector = Vector<float>.Build.DenseOfArray(new[] {3f, 4f});
            Assert.AreEqual(Vector<float>.Build.DenseOfArray(new[] {4f, -3f}), vector.Normal());
        }

        [TestMethod]
        public void TestVectorAngle()
        {
            Vector<float> v1 = Vector<float>.Build.DenseOfArray(new[] { 0, 1f });
            Vector<float> v2 = Vector<float>.Build.DenseOfArray(new[] { 1f, 1f });
            Vector<float> v3 = Vector<float>.Build.DenseOfArray(new[] { 1f, 0f });
            Assert.AreEqual(Convert.ToSingle(Math.PI/4), v1.GetAngle(v2));
            Assert.AreEqual(Convert.ToSingle(Math.PI/2), v1.GetAngle(v3));
        }

        [TestMethod]
        public void TestVectorIntersection()
        {
            Vector<float> p1 = Vector<float>.Build.DenseOfArray(new[] { 0f, 0f });
            Vector<float> p2 = Vector<float>.Build.DenseOfArray(new[] { 10f, 0f });
            Vector<float> v1 = Vector<float>.Build.DenseOfArray(new[] { 0, 1f });
            Vector<float> v2 = Vector<float>.Build.DenseOfArray(new[] { 1f, 1f });
            Assert.AreEqual(Vector<float>.Build.DenseOfArray(new[] {0f, -10f}), v1.IntersectionPoint(v2, p2, p1));
        }
    }
}
