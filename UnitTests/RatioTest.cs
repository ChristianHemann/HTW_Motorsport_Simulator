using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImportantClasses;
using static ImportantClasses.MathHelper;

namespace UnitTests
{
    [TestClass]
    public class RatioTest
    {
        [TestMethod]
        public void Testratio()
        {
            Ratio r = new MathHelper.Ratio(0.5, 0.5);
            double firstV = r.FirstValue;
            double secondeV = r.SecondValue;
            Assert.AreEqual(0.5f, firstV);
            Assert.AreEqual(0.5f, secondeV);

        }
    }
}
