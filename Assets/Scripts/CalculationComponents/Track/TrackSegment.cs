
using System.Xml.Serialization;
using MathNet.Numerics.LinearAlgebra;

namespace CalculationComponents.TrackComponents
{
    public class TrackSegment
    {
        [XmlIgnore] //it is not saved to avoid endless loops
        public TrackSegment PreviousTrackSegment { get; set; }
        
        public float TrackWidthEnd { get; set; }
        public virtual Vector<float> EndDirection { get { return _endDirection; } }
        public virtual Vector<float> EndPoint { get { return _endPoint; } set { _endPoint = value; } }

        protected Vector<float> _endPoint;
        protected Vector<float> _endDirection;

        protected TrackSegment(TrackSegment previousTrackSegment, float trackWidthEnd, Vector<float> endPoint, Vector<float> endDirection)
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
    }
}
