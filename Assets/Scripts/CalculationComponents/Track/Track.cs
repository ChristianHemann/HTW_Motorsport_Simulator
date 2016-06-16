using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents.TrackComponents;
using ImportantClasses;
using MathNet.Numerics.LinearAlgebra;

namespace CalculationComponents
{
    public class Track : ICalculationComponent
    {
        [ContainSettings("Track")]
        public static Track Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Track();
                return _instance;
            }
        }

        private static Track _instance;
        [Setting("Distance between cones")]
        public float ConeDistance { get; set; }

        [Setting("Name")]
        public string Name { get; set; }

        public TimeSpan BestRoundTime { get; set; }
        public TimeSpan LastRoundTime { get; set; }

        public List<TrackSegment> TrackSegments
        {
            get
            {
                return _trackSegments;
            }
            set
            {
                _trackSegments = value;
                if (value.Count > 1 && value.Last().PreviousTrackSegment == null) //set the previous TrackSegment of each TrackSegment because it is not saved to avoid endless loops
                    SetPreviousTrackSegments();
            }
        }

        private List<TrackSegment> _trackSegments;

        public Track()
        {
            TrackSegments = new List<TrackSegment>();
            Name = "";
            ConeDistance = 1.5f;
            TrackSegments.Add(new StartLine(null, 5, Vector<float>.Build.DenseOfArray(new[] {0f, 0f}),
                Vector<float>.Build.DenseOfArray(new[] {1f, 0f})));
            TrackSegments.Add(new Straight(TrackSegments.Last(),5, Vector<float>.Build.DenseOfArray(new[] { 10f, 0f })));
            TrackSegments.Add(new Curve(TrackSegments.Last(),5, Vector<float>.Build.DenseOfArray(new[] { 20f, 10f })));
        }

        public Vector<float>[] GetConePositions()
        {
            List<Vector<float>> positionList = new List<Vector<float>>();
            float distanceWithoutCones = 0; //the distance since the last cone
            foreach (TrackSegment trackSegment in TrackSegments)
            {
                Type segmentType = trackSegment.GetType();
                if (segmentType == typeof(Straight))
                {
                    Vector<float> widthVector = trackSegment.EndDirection.Normal().Normalize(2)*
                                                trackSegment.TrackWidthEnd/2;
                    float distance = -distanceWithoutCones + ConeDistance;
                    Straight straight = (Straight) trackSegment;
                    while (distance <= straight.Length)
                    {
                        positionList.Add(trackSegment.EndDirection.Normalize(2) * distance +
                                         trackSegment.PreviousTrackSegment.EndPoint + widthVector);
                        positionList.Add(trackSegment.EndDirection.Normalize(2) * distance +
                                         trackSegment.PreviousTrackSegment.EndPoint - widthVector);
                        distance += ConeDistance;
                    }
                    distanceWithoutCones = straight.Length - distance + ConeDistance;
                }
                else if(segmentType == typeof(Curve))
                {
                    Curve curve = (Curve) trackSegment;
                    float distance = -distanceWithoutCones + ConeDistance;
                    float lenght = curve.Angle*curve.Radius;
                    while (distance <= lenght)
                    {
                        positionList.Add(curve.GetPointAtAngle(distance/curve.Radius, curve.Radius - curve.TrackWidthEnd / 2));
                        positionList.Add(curve.GetPointAtAngle(distance/curve.Radius, curve.Radius + curve.TrackWidthEnd / 2));
                        distance += ConeDistance;
                    }
                    distanceWithoutCones = lenght - distance + ConeDistance;
                }
                else if (segmentType == typeof(StartLine))
                {
                    Vector<float> widthVector = trackSegment.EndDirection.Normal().Normalize(2)*trackSegment.TrackWidthEnd/2;
                    positionList.Add(trackSegment.EndPoint + widthVector);
                    positionList.Add(trackSegment.EndPoint - widthVector);
                    distanceWithoutCones = 0;
                }
            }
            return positionList.ToArray();
        }

        public void Calculate()
        {
            throw new NotImplementedException();
        }

        public void StopCalculation()
        {
            throw new NotImplementedException();
        }

        public void StoreResult()
        {
            throw new NotImplementedException();
        }

        private void SetPreviousTrackSegments()
        {
            TrackSegment previous = null;
            foreach (TrackSegment trackSegment in _trackSegments)
            {
                trackSegment.PreviousTrackSegment = previous;
                previous = trackSegment;
            }
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
