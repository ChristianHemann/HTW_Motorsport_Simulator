using System;
using ImportantClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class Vector3Test
    {
        private Vector3 v0, v1, v2, v3;
        private Vector2 vector2;

        [TestInitialize]
        public void Init()
        {
            v0 = new Vector3(0, 0, 0);
            v1 = new Vector3(1f, 0, 0.5f);
            v2 = new Vector3(0, 1f, 0.5f);
            v3 = new Vector3(1f, 1f, 1f);
            vector2 = new Vector2(1, 1);
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
            Assert.AreEqual(Math.Sqrt(3), v3.Magnitude, 1e-6);
        }

        [TestMethod]
        public void TestVectorNormalize()
        {
            Assert.AreEqual(1, v3.Normalize().Magnitude, 1e-6);
        }

        [TestMethod]
        public void TestVectorOperators()
        {
            Assert.AreEqual(v3, vector2.ToVector3() + new Vector3(0, 0, 1));
            Assert.AreEqual(v3, vector2 + new Vector3(0, 0, 1));
            Assert.AreEqual(v3, new Vector3(0, 0, 1) + vector2);
        }
    }
}
