using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImportantClasses;
using System.Xml.Serialization;

namespace UnitTests
{
    [TestClass]
    [Serializable]
    public class XmlTest
    {
        List<Spline> splineList = new List<Spline>();


        [TestMethod]
        public void TestObject()
        {
            Settings.SaveSettings();
            Settings.LoadSettings();
        }
    }
    
    [Serializable]
    public class ClassWithSettings
    {
        [XmlElement]
        [ContainSettings("bla")]
        public static Spline spl
        {
            get { return spline;}
            set { spline = value; }
        }
        [ContainSettings("Spline2")]
        public static Spline spline = new Spline(new[] { 1.0, 2.0, 3.0 }, new[] { 1.0, 4.0, 9.0 });
    }
}
