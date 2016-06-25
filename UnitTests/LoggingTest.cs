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
            DateTime actTime = DateTime.Now;
            Spline secondObject = new Spline(new[] {0.0, 1.0}, new[] {2.0, 3.0});
            Logging.Log("Loggingtest: some text to log", Logging.Classification.Message);
            Logging.Log(secondObject, Logging.Classification.CalculationResult,Message.MessageCode.Error);

            //Test correct saving and Filter Date
            Logging[] logs = Logging.GetLogs(from: actTime, interval: TimeSpan.FromMinutes(1));
            Assert.AreEqual(2, logs.Length);
            Assert.AreEqual("Loggingtest: some text to log", logs[0].Value);
            Assert.AreEqual(Logging.Classification.Message, logs[0].classification);
            Assert.AreEqual(Message.MessageCode.Notification, logs[0].Code);
            Assert.AreEqual(secondObject.ToString(), logs[1].Value);
            Assert.AreEqual(Logging.Classification.CalculationResult, logs[1].classification);
            Assert.AreEqual(Message.MessageCode.Error, logs[1].Code);

            //test Filter classification
            logs = Logging.GetLogs(Logging.Classification.CalculationResult, from: actTime,
                interval: TimeSpan.FromMinutes(1));
            Assert.AreEqual(1, logs.Length);

            //test filter messageCode
            logs = Logging.GetLogs(code:Message.MessageCode.Error, from: actTime,
                interval: TimeSpan.FromMinutes(1));
            Assert.AreEqual(1, logs.Length);

            //test Filter count
            logs = Logging.GetLogs(count: 1, from: actTime,
                interval: TimeSpan.FromMinutes(1));
            Assert.AreEqual(1, logs.Length);
            Assert.AreEqual("Loggingtest: some text to log", logs.First().Value);

            //test Filter startAt
            logs = Logging.GetLogs(startAt:1, from: actTime,
                interval: TimeSpan.FromMinutes(1));
            Assert.AreEqual(1, logs.Length);
            Assert.AreEqual(secondObject.ToString(), logs[0].Value);

            //test Filter date
            logs = Logging.GetLogs(from: DateTime.Now);
            Assert.AreEqual(0, logs.Length);
        }
    }
}
