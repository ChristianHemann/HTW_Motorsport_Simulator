
using System.Xml.Serialization;
using ImportantClasses;

namespace CalculationComponents.TrackComponents
{
    /// <summary>
    /// a segment of one Track
    /// </summary>
    [XmlInclude(typeof(Straight)),XmlInclude(typeof(Curve)),XmlInclude(typeof(StartLine))]
    public abstract class TrackSegment
    {
        /// <summary>
        /// the previous segment of the track
        /// </summary>
        [XmlIgnore] //it is not saved to avoid endless loops
        public TrackSegment PreviousTrackSegment
        {
            get { return _previousTrackSegment; }
            set
            {
                if (_previousTrackSegment != null)
                    _previousTrackSegment.TrackSegmentChanged -= PreviousTrackSegmentChanged;
                        //delete event of the old TrackSegment
                _previousTrackSegment = value;
                if (_previousTrackSegment != null)
                    _previousTrackSegment.TrackSegmentChanged += PreviousTrackSegmentChanged;
                        //add event of the the new TrackSegment
            }
        }

        /// <summary>
        /// the width of the TrackSegment at the end
        /// </summary>
        public float TrackWidthEnd { get; set; }

        /// <summary>
        /// the direction of the segment where it ends
        /// </summary>
        public virtual Vector2 EndDirection { get { return _endDirection; } set { _endDirection = value; } }

        /// <summary>
        /// the point where the segment ends
        /// </summary>
        public virtual Vector2 EndPoint
        {
            get { return _endPoint; }
            set
            {
                _endPoint = value;
                CallTrackSegmentChangedEvent();
            }
        }

        protected Vector2 _endPoint;
        protected Vector2 _endDirection;
        protected TrackSegment _previousTrackSegment;

        /// <summary>
        /// a segment of one track
        /// </summary>
        /// <param name="previousTrackSegment">the previous segment of the track</param>
        /// <param name="trackWidthEnd">the width of the Tracksegment where it ends</param>
        /// <param name="endPoint">the point where the segment ends</param>
        /// <param name="endDirection">the direction of the segment where it ends</param>
        protected TrackSegment(TrackSegment previousTrackSegment, float trackWidthEnd, Vector2 endPoint, Vector2 endDirection)
        {
            PreviousTrackSegment = previousTrackSegment;
            TrackWidthEnd = trackWidthEnd;
            _endDirection = endDirection;
            _endPoint = endPoint;
        }

        /// <summary>
        /// calculates values which are defined in the classes which inherits from TrackSegment according to the properties defined in TrackSegment
        /// </summary>
        protected virtual void CalculateDerivedValues()
        {
            
        }

        /// <summary>
        /// calculates properties which are defined in TrackSegment according to the values in a class which inherits from TrackSegment
        /// </summary>
        protected virtual void CalculateBaseValues()
        {
            
        }

        /// <summary>
        /// invoked by the TrackSegmentChanged event of the PreviousTrackSegment
        /// </summary>
        protected virtual void PreviousTrackSegmentChanged()
        {

        }

        /// <summary>
        /// used to call the event TrackSegmentChanged from derived classes
        /// </summary>
        protected void CallTrackSegmentChangedEvent()
        {
            if (TrackSegmentChanged != null)
                TrackSegmentChanged();
        }

        /// <summary>
        /// used to tell the following TrackSegment that its start position changed
        /// </summary>
        public delegate void TrackSegmentChangedDelegate();

        /// <summary>
        /// invoked when the EndPoint or EndDirection changed
        /// </summary>
        public event TrackSegmentChangedDelegate TrackSegmentChanged;
    }
}
