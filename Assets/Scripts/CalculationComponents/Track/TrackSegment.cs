using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CalculationComponents.TrackComponents
{
    public class TrackSegment
    {
        public TrackSegment PreviousTrackSegment { get; set; }

        public float TrackWidthStart { get; set; }
        public float TrackWidthEnd { get; set; }
        public virtual Vector2 EndDirection { get { return _endDirection; } }
        public Vector2 EndPoint { get; set; }

        protected Vector2 _endDirection;

        protected TrackSegment(TrackSegment previousTrackSegment, float trackWidthStart, float trackWidthEnd, Vector2 endPoint, Vector2 endDirection)
        {
            PreviousTrackSegment = previousTrackSegment;
            TrackWidthEnd = trackWidthEnd;
            TrackWidthStart = trackWidthStart;
            _endDirection = endDirection;
            EndPoint = endPoint;
        }

        protected virtual void ReCalculate()
        {
            
        }
    }
}
