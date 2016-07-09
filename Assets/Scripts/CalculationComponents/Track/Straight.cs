
using System;
using System.Xml.Serialization;
using ImportantClasses;

namespace CalculationComponents.TrackComponents
{
    /// <summary>
    /// a straight segment of a track
    /// </summary>
    public sealed class Straight : TrackSegment
    {
        /// <summary>
        /// the length of the straight
        /// </summary>
        public float Length
        {
            get { return _lenght; }
            set
            {
                _lenght = value;
                if(PreviousTrackSegment!=null)
                    CalculateBaseValues();
            }
        }

        /// <summary>
        /// the point where the segment ends
        /// </summary>
        public override Vector2 EndPoint
        {
            get
            {
                return _endPoint;
            }
            set
            {
                if (PreviousTrackSegment == null)
                    _endPoint = value;
                else
                {
                    //if the endpoint is not in line with the direction of the segment
                    Vector2 newDir = (value - PreviousTrackSegment.EndPoint).Normalize();
                    if (!newDir.Equals(PreviousTrackSegment.EndDirection.Normalize(), (float) 1E-5))
                    {
                        throw new ArgumentException("The endpoint is not in line with the direction.  " +
                                                    value.ToString());
                    }
                    _endPoint = value;
                    CalculateDerivedValues();
                    CallTrackSegmentChangedEvent();
                }
            }
        }

        /// <summary>
        /// the direction of the segment where it ends
        /// </summary>
        [XmlIgnore]
        public override Vector2 EndDirection
        {
            get { return PreviousTrackSegment.EndDirection; }
        }

        private float _lenght;

        /// <summary>
        /// a straight segment of a track
        /// </summary>
        /// <param name="previousTrackSegment">the previous segment of the track</param>
        /// <param name="trackWidthEnd">the width of the Tracksegment where it ends</param>
        /// <param name="length">the length of the straight</param>
        public Straight(TrackSegment previousTrackSegment, float trackWidthEnd, float length)
            : base(previousTrackSegment, trackWidthEnd, previousTrackSegment.EndDirection.Normalize()*length, previousTrackSegment.EndDirection)
        {
            _lenght = length;
        }

        private Straight() : base(null, 0, null, null) { } //just for Xml-Storing

        /// <summary>
        /// calculates values which are defined in the classes which inherits from TrackSegment according to the properties defined in TrackSegment
        /// </summary>
        protected override void CalculateDerivedValues()
        {
            _lenght = (EndPoint - PreviousTrackSegment.EndPoint).Magnitude;
        }

        /// <summary>
        /// calculates properties which are defined in TrackSegment according to the values in a class which inherits from TrackSegment
        /// </summary>
        protected override void CalculateBaseValues()
        {
            EndPoint = PreviousTrackSegment.EndPoint + _lenght * EndDirection.Normalize();
        }

        /// <summary>
        /// invoked by the TrackSegmentChanged event of the PreviousTrackSegment
        /// </summary>
        protected override void PreviousTrackSegmentChanged()
        {
            CalculateBaseValues();
        }
    }
}
