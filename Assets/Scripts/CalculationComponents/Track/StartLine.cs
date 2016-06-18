using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ImportantClasses;
using MathNet.Numerics.LinearAlgebra;

namespace CalculationComponents.TrackComponents
{
    /// <summary>
    /// The start/FinishLine of a Track
    /// </summary>
    public sealed class StartLine : TrackSegment
    {
        /// <summary>
        /// The start/FinishLine of a Track
        /// </summary>
        /// <param name="previousTrackSegment">the previous segment of the track</param>
        /// <param name="trackWidthEnd">the width of the Tracksegment where it ends</param>
        /// <param name="endPoint">the point where the segment ends</param>
        /// <param name="endDirection">the direction of the segment where it ends</param>
        public StartLine(TrackSegment previousTrackSegment, float trackWidthEnd, Vector2 endPoint, Vector2 endDirection) 
            : base(previousTrackSegment, trackWidthEnd, endPoint, endDirection)
        {
            
        }

        private StartLine() : base(null, 5f, null, null) { } //just for xml-storing
        
        /// <summary>
        /// sets the EndDirection if there is no PreviousTrackSegment
        /// </summary>
        /// <param name="endDirecion">the new EndDirection</param>
        /// <returns>true if it was successfully set</returns>
        public bool TrySetEndDirection(Vector2 endDirecion)
        {
            if (PreviousTrackSegment != null)
                return false;
            _endDirection = endDirecion;
            CallTrackSegmentChangedEvent();
            return true;
        }

        /// <summary>
        /// invoked by the TrackSegmentChanged event of the PreviousTrackSegment
        /// </summary>
        protected override void PreviousTrackSegmentChanged()
        {
            _endDirection = PreviousTrackSegment.EndDirection;
            EndPoint = PreviousTrackSegment.EndPoint;
        }
    }
}
