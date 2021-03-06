﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulator;
using ImportantClasses;
using CalculationComponents;
using Output;

namespace UnitTests
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für BrakeTest1
    /// </summary>
    [TestClass]
    public class BrakeTest1
    {
          public BrakeTest1()
        {
            //
            // TODO: Konstruktorlogik hier hinzufügen
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Ruft den Textkontext mit Informationen über
        ///den aktuellen Testlauf sowie Funktionalität für diesen auf oder legt diese fest.
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

        #region Zusätzliche Testattribute
        //
        // Sie können beim Schreiben der Tests folgende zusätzliche Attribute verwenden:
        //
        // Verwenden Sie ClassInitialize, um vor Ausführung des ersten Tests in der Klasse Code auszuführen.
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Verwenden Sie ClassCleanup, um nach Ausführung aller Tests in einer Klasse Code auszuführen.
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Mit TestInitialize können Sie vor jedem einzelnen Test Code ausführen. 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Mit TestCleanup können Sie nach jedem Test Code ausführen.
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Testbrake()
        {
            Brake br = new Brake();
            br.NormalBrakeMoment = 100;
            br.BrakeBalance = new MathHelper.Ratio(0.5f, 0.5f);
            InputData.UsedInputData = new InputData(0, 0, 0, 0);
            InputData.UsedInputData.BrakePedal = 0.5f;
            br.Calculate();
            br.StoreResult();
            Assert.AreEqual(50,BrakeOutput.LastCalculation.BrakeMomentFront);
           Assert.AreEqual(50,BrakeOutput.LastCalculation.BrakeMomentRear);



        }

    }
}
