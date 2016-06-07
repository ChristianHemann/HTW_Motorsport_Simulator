using System;
using ImportantClasses;
using MathNet.Numerics.LinearAlgebra;

namespace CalculationComponents.TrackComponents
{
    public sealed class Curve : TrackSegment
    {
        public float Angle { get; set; }
        public float Radius { get; set; }

        private Vector<float> _middlePoint;

        public Curve(TrackSegment previousTrackSegment, float trackWidthStart, float trackWidthEnd, Vector<float> endPoint, Vector<float> endDirection)
            : base(previousTrackSegment ,trackWidthStart, trackWidthEnd, endPoint, endDirection)
        {
            if (endPoint.Equals(previousTrackSegment.EndPoint)) //complete circle
            {
                Radius = (trackWidthEnd + trackWidthStart)/2;
                Angle = Convert.ToSingle(2*Math.PI);
                _middlePoint = previousTrackSegment.EndDirection.Normal().Normalize(2)*Radius +
                               previousTrackSegment.EndPoint;
            }
            else
            {
                //calculate middle point
                Vector<float> connectingVector = (previousTrackSegment.EndPoint - endPoint)/2; //2 to get the middle of the distance
                Vector<float> normalVectorMiddle = connectingVector.Normal();
                _middlePoint = normalVectorMiddle.IntersectionPoint(previousTrackSegment.EndDirection.Normal(),
                    previousTrackSegment.EndPoint + connectingVector, previousTrackSegment.EndPoint);

                //calculate radius
                Radius = (endPoint - _middlePoint).GetMagnitude();
                //calculate angle
                Angle = normalVectorMiddle.GetAngle(endDirection.Normal())*2;
            }
        }

        protected override void CalculateDerivedValues()
        {
            base.CalculateDerivedValues();
        }

        protected override void CalculateBaseValues()
        {
            base.CalculateBaseValues();
        }
    }
}
