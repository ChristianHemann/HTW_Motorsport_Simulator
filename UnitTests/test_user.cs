using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator;

namespace UnitTests
{
    /// <summary>
    /// Summary description for test_user
    /// </summary>
    [TestClass]
    public class test_user
    {
        public test_user()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            User.Instance.set_Round_time("loic", new TimeSpan(1,20,30));
            User.Instance.set_Round_time("christian", new TimeSpan(2, 45, 52));
            User.Instance.set_Round_time("testat", new TimeSpan(1, 10, 54));
            User.Instance.set_Round_time("loic", new TimeSpan(3, 20, 30));
            User.Instance.set_Round_time("loic", new TimeSpan(0, 20, 00));

            TimeSpan best_time = User.Instance.Best_round_times["loic"];

            Assert.AreEqual(best_time, new TimeSpan(0, 20, 00));

        }
    }
}
