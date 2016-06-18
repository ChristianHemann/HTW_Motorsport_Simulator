using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CalculationComponents;
using CalculationComponents.TrackComponents;
using ImportantClasses;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class TrackTest
    {
        List<Vector2> positions = new List<Vector2>(); //conePositions

        [TestInitialize]
        public void InitTrackTest()
        {

            Track.Instance.ConeDistance = 5;
            Track.Instance.TrackSegments.Clear();
            Track.Instance.TrackSegments.Add(new StartLine(null, 5, new Vector2(0,0), new Vector2(1,0)));
            Track.Instance.TrackSegments.Add(new Straight(Track.Instance.TrackSegments.Last(), 5, 10f));
            Track.Instance.TrackSegments.Add(new Curve(Track.Instance.TrackSegments.Last(), 5, new Vector2(20,10)));

            //cones startline
            positions.Add(new Vector2(0, -2.5f));
            positions.Add(new Vector2(0, 2.5f));
            //cones straight
            positions.Add(new Vector2(5, -2.5f));
            positions.Add(new Vector2(5, 2.5f));
            positions.Add(new Vector2(10, -2.5f));
            positions.Add(new Vector2(10, 2.5f));
            //cones curve
            Vector2 point = new Vector2(10,10);
            positions.Add(point + 12.5f * new Vector2((float)Math.Cos(1.5 * Math.PI + 0.5), (float)Math.Sin(1.5 * Math.PI + 0.5)));
            positions.Add(point + 7.5f * new Vector2((float)Math.Cos(1.5 * Math.PI + 0.5), (float)Math.Sin(1.5 * Math.PI + 0.5)));
            positions.Add(point + 12.5f * new Vector2((float)Math.Cos(1.5 * Math.PI + 1), (float)Math.Sin(1.5 * Math.PI + 1)));
            positions.Add(point + 7.5f * new Vector2((float)Math.Cos(1.5 * Math.PI + 1), (float)Math.Sin(1.5 * Math.PI + 1)));
            positions.Add(point + 12.5f * new Vector2((float)Math.Cos(1.5 * Math.PI + 1.5), (float)Math.Sin(1.5 * Math.PI + 1.5)));
            positions.Add(point + 7.5f * new Vector2((float)Math.Cos(1.5 * Math.PI + 1.5), (float)Math.Sin(1.5 * Math.PI + 1.5)));
        }

        [TestMethod]
        public void TestTrackConePositions()
        {
            Vector2[] trackConePositions = Track.Instance.GetConePositions();
            Vector2[] expectedConePositions = positions.ToArray();
            Assert.AreEqual(expectedConePositions.Length, trackConePositions.Length);
            for (int i = 0; i < trackConePositions.Length; i++)
            {
                Assert.IsTrue(expectedConePositions[i].Equals(trackConePositions[i],(float)1e-5));
            }
        }
    }
}
