using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImportantClasses;
using System.Xml.Serialization;

namespace UnitTests
{
    [TestClass]
    [Serializable]
    public class SettingsTest
    {
        List<Spline> splineList = new List<Spline>();


        [TestMethod]
        public void TestSaveAndLoad()
        {
            Settings.Initialize();
            Settings.SaveAllSettings();
            ClassWithSettings.spline = new Spline(new[] { 0.0, 1.0, 2.0 }, new[] { 1.0, 4.0, 9.0 }); //Check if the old spline is really loaded
            Settings.LoadAllSettings();
            double value = ClassWithSettings.spline.Interpolate(1.5);
            Assert.AreEqual(2.3125, value);
        }

        [TestMethod]
        public void TestGetMenuItems()
        {
            //Test passed at 03.05.2016
            //The Length oh the returned List can change, so that you have to count all the attributes manually for comparision
            Settings.Initialize();
            int len = Settings.GetMenuItems(new string[0]).Count;
            Assert.AreEqual(len, 3); //this value is hardCoded. it can change when changing the number of attributes in the code
        }
    }
    
    public class ClassWithSettings
    {
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
