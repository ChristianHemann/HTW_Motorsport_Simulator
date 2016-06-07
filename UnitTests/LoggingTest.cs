using System;
using System.IO;
using System.Linq;
using ImportantClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class LoggingTest
    {
        [TestMethod]
        public void WriteStringLog()
        {
            Logging.Log("some text to log",Logging.Classification.Message,Message.MessageCode.Notification);
            //the Result of the stored data cannot be checked automatically, because the exacly name of the file is not clear in this class
        }

        [TestMethod]
        public void WriteObjectLog()
        {
            Logging.Log(new Spline(new []{0.0,1.0},new []{1.0,2.0}),Logging.Classification.CalculationResult,Message.MessageCode.Warning );
        }
    }
}
