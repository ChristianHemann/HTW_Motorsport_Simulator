using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportantClasses;
using UnityEngine;

namespace CalculationComponents.TrackComponents
{
    public class Curve : TrackSegment
    {
        public float Angle { get; set; }
        public float Radius { get; set; }

        private Vector2 _middlePoint;

        public Curve(TrackSegment previousTrackSegment, float trackWidthStart, float trackWidthEnd, Vector2 endPoint, Vector2 endDirection)
            : base(previousTrackSegment ,trackWidthStart, trackWidthEnd, endPoint, endDirection)
        {
            if (endPoint.Equals(previousTrackSegment.EndPoint)) //complete circle
            {
                Radius = (trackWidthEnd + trackWidthStart)/2;
                Angle = 360;
                _middlePoint = previousTrackSegment.EndDirection.Normal().normalized*Radius +
                               previousTrackSegment.EndPoint;
            }
            else
            {
                //calculate middle point
                Vector2 connectingVector = (previousTrackSegment.EndPoint - endPoint)/2; //2 to get the middle of the distance
                Vector2 normalVectorMiddle = connectingVector.Normal();
                _middlePoint = normalVectorMiddle.IntersectionPoint(previousTrackSegment.EndDirection.Normal(),
                    previousTrackSegment.EndPoint + connectingVector, previousTrackSegment.EndPoint);

                //calculate radius
                Radius = (endPoint - _middlePoint).magnitude;
                //calculate angle
                Angle = normalVectorMiddle.GetAngle(endDirection.Normal())*2;
            }
        }
    }
}
