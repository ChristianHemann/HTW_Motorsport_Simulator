using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CalculationComponents.TrackComponents;
using ImportantClasses;

namespace CalculationComponents
{
    /// <summary>
    /// a racetrack to drive on with the car
    /// </summary>
    public class Track : ICalculationComponent
    {
        /// <summary>
        /// the actually used track
        /// </summary>
        [ContainSettings("Track")]
        public static Track Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Track();
                return _instance;
            }
            set
            {
                Settings.FileLoaded -= _instance.SetPreviousTrackSegments;
                _instance = value;
                Settings.FileLoaded += _instance.SetPreviousTrackSegments;
            } //The setter is just to load a value via reflection
        }

        /// <summary>
        /// the distance betweeen 2 cones
        /// </summary>
        [Setting("Distance between cones")]
        public float ConeDistance { get; set; }

        /// <summary>
        /// the name of the track
        /// </summary>
        [Setting("Name")]
        public string Name { get; set; }

        /// <summary>
        /// the fastest laptime on this track
        /// </summary>
        public TimeSpan BestRoundTime { get; set; }
        /// <summary>
        /// the laptime of the last lap on this track
        /// </summary>
        [XmlIgnore]
        public TimeSpan LastRoundTime { get; set; }

        /// <summary>
        /// contains all TrackSegments that the track consists of
        /// </summary>
        public List<TrackSegment> TrackSegments
        {
            get
            {
                return _trackSegments;
            }
            set
            {
                _trackSegments = value;
                //if (value.Count > 1 && value.Last().PreviousTrackSegment == null) //set the previous TrackSegment of each TrackSegment because it is not saved to avoid endless loops
                //    SetPreviousTrackSegments();
            }
        }

        private static Track _instance;
        private List<TrackSegment> _trackSegments;

        /// <summary>
        /// a racetrack to drive on with the car
        /// </summary>
        private Track()
        {
            Name = "";
            ConeDistance = 1f;
            TrackSegments = new List<TrackSegment>();
            //TrackSegments.Add(new StartLine(null, 5, new Vector2(0, 0), new Vector2(1, 0)));
            //TrackSegments.Add(new Straight(TrackSegments.Last(), 5, 10f));
            //TrackSegments.Add(new Curve(TrackSegments.Last(), 5, new Vector2(20, 10)));
        }

        /// <summary>
        /// calculates the position of each cone which stands on the track
        /// </summary>
        /// <returns>the positions of the cones</returns>
        public Vector2[] GetConePositions()
        {
            List<Vector2> positionList = new List<Vector2>();
            float distanceWithoutCones = 0; //the distance since the last cone
            foreach (TrackSegment trackSegment in TrackSegments)
            {
                Type segmentType = trackSegment.GetType();
                if (segmentType == typeof(Straight)) //gets the cone positions for a straight
                {
                    Vector2 widthVector = trackSegment.EndDirection.Normal().Normalize() *
                                                trackSegment.TrackWidthEnd / 2;
                    float distance = -distanceWithoutCones + ConeDistance;
                    Straight straight = (Straight)trackSegment;
                    while (distance <= straight.Length)
                    {
                        positionList.Add(trackSegment.EndDirection.Normalize() * distance +
                                         trackSegment.PreviousTrackSegment.EndPoint + widthVector);
                        positionList.Add(trackSegment.EndDirection.Normalize() * distance +
                                         trackSegment.PreviousTrackSegment.EndPoint - widthVector);
                        distance += ConeDistance;
                    }
                    distanceWithoutCones = straight.Length - distance + ConeDistance;
                }
                else if (segmentType == typeof(Curve)) //get the cone positions of a curve
                {
                    Curve curve = (Curve)trackSegment;
                    float distance = -distanceWithoutCones + ConeDistance;
                    float lenght = curve.Angle * curve.Radius;
                    while (distance <= lenght)
                    {
                        positionList.Add(curve.GetPointAtAngle(distance / curve.Radius, curve.Radius + curve.TrackWidthEnd / 2));
                        positionList.Add(curve.GetPointAtAngle(distance / curve.Radius, curve.Radius - curve.TrackWidthEnd / 2));
                        distance += ConeDistance;
                    }
                    distanceWithoutCones = lenght - distance + ConeDistance;
                }
                else if (segmentType == typeof(StartLine)) //get the cone positions of a start line
                {
                    Vector2 widthVector = trackSegment.EndDirection.Normal().Normalize() * trackSegment.TrackWidthEnd / 2;
                    positionList.Add(trackSegment.EndPoint + widthVector);
                    positionList.Add(trackSegment.EndPoint - widthVector);
                    distanceWithoutCones = 0;
                }
            }
            return positionList.ToArray();
        }

        /// <summary>
        /// calculates the change of the height of the track
        /// </summary>
        public void Calculate()
        {
            //since there are no bumps on the track jet here is nothing to calculate
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if running
        /// </summary>
        public void StopCalculation()
        {
            //since there are no bumps on the track jet here is nothing to calculate
        }

        /// <summary>
        /// stores the calculation results of the track to the TrackOutput class
        /// </summary>
        public void StoreResult()
        {
            //since there are no bumps on the track, here is nothing to calculate
        }

        /// <summary>
        /// this function is not neede for the Track
        /// </summary>
        public void CalculateBackwards()
        {
            //here is nothing to do
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// used when loading from a xml-file. Sets the previous TrackSegment of each TrackSegment, because it is not saved
        /// </summary>
        private void SetPreviousTrackSegments(Type type)
        {
            if (type == typeof(Track))
            {
                TrackSegment previous = null;
                foreach (TrackSegment trackSegment in _trackSegments)
                {
                    trackSegment.PreviousTrackSegment = previous;
                    previous = trackSegment;
                }
                if (_trackSegments.Count > 0)
                    _trackSegments.First().CallTrackSegmentChangedEvent();
            }
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
