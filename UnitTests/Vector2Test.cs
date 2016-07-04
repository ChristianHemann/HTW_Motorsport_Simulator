using System;
using ImportantClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class Vector2Test
    {
        private Vector2 v0, v1, v2, v3;

        [TestInitialize]
        public void Init()
        {
            v0 = new Vector2(0, 0);
            v1 = new Vector2(1, 0);
            v2 = new Vector2(0, 1);
            v3 = new Vector2(1, 1);
        }
        [TestMethod]
        public void TestVectorEqual()
        {
            Assert.IsTrue(v0.Equals(v0));
            Assert.IsTrue(v0.Equals(v3, 1f));
            Assert.IsTrue(v3.Equals(v0, 1f));
            Assert.IsFalse(v0.Equals(v3, 0.999f));
        }

        [TestMethod]
        public void TestVectorMagnitude()
        {
            Assert.AreEqual(Math.Sqrt(2), v3.Magnitude, 1e-6);
        }

        [TestMethod]
        public void TestVectorAngle()
        {
            Assert.AreEqual(Math.PI / 4.0, v3.GetEnclosedAngle(v1), 1e-6);
            Assert.AreEqual(Math.PI / 2.0, v1.GetEnclosedAngle(v2), 1e-6);
        }

        [TestMethod]
        public void TestVectorIntersectionPoint()
        {
            Assert.AreEqual(new Vector2(-1, 0), v3.GetIntersectionPoint(v1, v1, v2));
        }

        [TestMethod]
        public void TestVectorNormalize()
        {
            Assert.AreEqual(1, v3.Normalize().Magnitude, 1e-6);
        }

        [TestMethod]
        public void TestVectorNormal()
        {
            Assert.AreEqual(v1,v2.Normal());
        }

        [TestMethod]
        public void TestVectorTurn()
        {
            Assert.IsTrue(v2.Equals(v1.Turn(Math.PI / 2),1e-5f));
        }
    }
}
