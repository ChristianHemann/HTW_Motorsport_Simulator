using System;
using System.Xml.Serialization;
using ImportantClasses;
using MathNet.Numerics.LinearAlgebra;

namespace CalculationComponents.TrackComponents
{
    public sealed class Curve : TrackSegment
    {
        public enum CornerType
        {
            LeftCorner,
            RightCorner
        }

        [XmlIgnore]
        public CornerType Type { get { return _type; } }
        [XmlIgnore]
        public float Angle { get {return _angle;} set { _angle = value; CalculateBaseValues(); } }
        [XmlIgnore]
        public float Radius { get {return _radius;} set { _radius = value; CalculateBaseValues(); } }
        [XmlIgnore]
        public Vector<float> MiddlePoint { get { return _middlePoint; } }

        private CornerType _type;
        private float _angle, _radius;
        private Vector<float> _middlePoint;
        private float _startingAngle; //the angle of the direction vector at the beginning of the curve in polar coordinates to the middlePoint

        public Curve(TrackSegment previousTrackSegment, float trackWidthEnd, Vector<float> endPoint)
            : base(previousTrackSegment ,trackWidthEnd, endPoint, null)
        {
            CalculateDerivedValues();
            _endDirection = GetDirectionAtAngle(_angle);
        }

        /// <summary>
        /// gets the direction vector on the curve at the angle phi which runs from the startPoint
        /// </summary>
        /// <param name="phi">the angle</param>
        /// <returns></returns>
        public Vector<float> GetDirectionAtAngle(float phi)
        {
            phi += _startingAngle;
            return 
                _radius * Vector<float>.Build.DenseOfArray(new[]
                {Convert.ToSingle(-Math.Sin(phi)), Convert.ToSingle(Math.Cos(phi))});
        }

        /// <summary>
        /// gets the point on the curve at the angle phi which runs from the startPoint
        /// </summary>
        /// <param name="phi">the angle</param>
        /// <param name="radius">if a radius other than the curves one shall be used</param>
        /// <returns></returns>
        public Vector<float> GetPointAtAngle(float phi, float radius = 0)
        {
            if (radius.Equals(0))
                radius = _radius;
            phi += _startingAngle;
            return _middlePoint +
                   radius * Vector<float>.Build.DenseOfArray(new[]
                   {Convert.ToSingle(Math.Cos(phi)), Convert.ToSingle(Math.Sin(phi))});
        }

        private Curve() : base(null, 0, null, null) { } //just for Xml-Storing

        protected override void CalculateDerivedValues()
        {
            if (EndPoint.Equals(PreviousTrackSegment.EndPoint)) //complete circle
            {
                Radius = (TrackWidthEnd + PreviousTrackSegment.TrackWidthEnd) / 2;
                Angle = Convert.ToSingle(2 * Math.PI);
                _middlePoint = PreviousTrackSegment.EndDirection.Normal().Normalize(2) * Radius +
                               PreviousTrackSegment.EndPoint;
            }
            else
            {
                //calculate middle point
                Vector<float> connectingVector = (EndPoint-PreviousTrackSegment.EndPoint) / 2; //2 to get the middle of the distance
                Vector<float> normalVectorMiddle = connectingVector.Normal();
                _middlePoint = normalVectorMiddle.IntersectionPoint(PreviousTrackSegment.EndDirection.Normal(),
                    PreviousTrackSegment.EndPoint, PreviousTrackSegment.EndPoint + connectingVector);

                //calculate radius
                _radius = (EndPoint - _middlePoint).GetMagnitude();
                //calculate angle
                _angle = normalVectorMiddle.GetAngle(_endPoint-_middlePoint) * 2;
            }

            //CornerType
            //c_ = k*a_ + h*b_ = EndPoint-StartPoint
            //if h>0 => right else left
            Vector<float> c = EndPoint - PreviousTrackSegment.EndPoint;
            Vector<float> a = PreviousTrackSegment.EndDirection;
            Vector<float> b = a.Normal();
            if ((a.At(0)*c.At(1)-c.At(0))/(b.At(1)*a.At(0)-b.At(0)-a.At(1))>0)
                _type = CornerType.RightCorner;
            else
                _type = CornerType.LeftCorner;

            //startingAngle
            float x = _middlePoint.At(0) - PreviousTrackSegment.EndPoint.At(0);
            float y = _middlePoint.At(1) - PreviousTrackSegment.EndPoint.At(1);
            if (x > 0)
                _startingAngle = Convert.ToSingle(Math.Atan(y/x));
            else if (x.Equals(0))
            {
                if (y >= 0)
                    _startingAngle = Convert.ToSingle(Math.Atan(y/x) + Math.PI);
                else
                    _startingAngle = Convert.ToSingle(Math.Atan(y / x) - Math.PI);
            }
            else
            {
                if (y >= 0)
                    _startingAngle = Convert.ToSingle(Math.PI / 2);
                else
                    _startingAngle = -Convert.ToSingle(Math.PI / 2);
            }
        }

        protected override void CalculateBaseValues()
        {
            _endPoint = GetPointAtAngle(_angle);
            _endDirection = GetDirectionAtAngle(_angle);
        }
    }
}
