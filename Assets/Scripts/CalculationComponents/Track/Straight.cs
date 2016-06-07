using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CalculationComponents.TrackComponents
{
    public class Straight : TrackSegment
    {
        public float Length
        {
            get { return _lenght; }
            set
            {
                _lenght = value;
                ReCalculate();
            }
        }

        public override Vector2 EndDirection
        {
            get { return PreviousTrackSegment.EndDirection; }
        }

        private float _lenght;

        public Straight(TrackSegment previousTrackSegment, float trackWidthStart, float trackWidthEnd, Vector2 endPoint, Vector2 endDirection)
            : base(previousTrackSegment, trackWidthStart, trackWidthEnd, endPoint, endDirection)
        {
            _lenght = (endPoint - previousTrackSegment.EndPoint).magnitude;
        }

        protected override void ReCalculate()
        {
            EndPoint = PreviousTrackSegment.EndPoint + _lenght*EndDirection;
        }
    }
}
