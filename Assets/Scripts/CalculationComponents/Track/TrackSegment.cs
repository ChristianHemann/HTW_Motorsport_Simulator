
using MathNet.Numerics.LinearAlgebra;

namespace CalculationComponents.TrackComponents
{
    public class TrackSegment
    {
        public TrackSegment PreviousTrackSegment { get; set; }

        public float TrackWidthStart { get; set; }
        public float TrackWidthEnd { get; set; }
        public virtual Vector<float> EndDirection { get { return _endDirection; } }
        public Vector<float> EndPoint { get; set; }

        protected Vector<float> _endDirection;

        protected TrackSegment(TrackSegment previousTrackSegment, float trackWidthStart, float trackWidthEnd, Vector<float> endPoint, Vector<float> endDirection)
        {
            PreviousTrackSegment = previousTrackSegment;
            TrackWidthEnd = trackWidthEnd;
            TrackWidthStart = trackWidthStart;
            _endDirection = endDirection;
            EndPoint = endPoint;
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
