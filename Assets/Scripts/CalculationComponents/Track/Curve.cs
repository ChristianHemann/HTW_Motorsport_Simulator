using System;
using System.Xml.Serialization;
using ImportantClasses;
using JetBrains.Annotations;

namespace CalculationComponents.TrackComponents
{
    /// <summary>
    /// A corner of a Track
    /// </summary>
    public sealed class Curve : TrackSegment
    {
        /// <summary>
        /// defines if it is a right corner or a left corner
        /// </summary>
        public enum CornerType
        {
            LeftCorner,
            RightCorner
        }

        /// <summary>
        /// defines if it is a right corner or a left corner
        /// </summary>
        [XmlIgnore]
        public CornerType Type { get { return _type; } }

        /// <summary>
        /// the angle of the curve
        /// </summary>
        [XmlIgnore]
        public float Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                CalculateBaseValues();
                CallTrackSegmentChangedEvent();
            }
        }
        /// <summary>
        /// the radius of the curve
        /// </summary>
        [XmlIgnore]
        public float Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                //calculate middle Point
                if (Type == CornerType.RightCorner)
                    _middlePoint = PreviousTrackSegment.EndDirection.Normal().Normalize() * value;
                else
                    _middlePoint = -PreviousTrackSegment.EndDirection.Normal().Normalize() * value;

                CalculateBaseValues();
                CallTrackSegmentChangedEvent();
            }
        }

        public override TrackSegment PreviousTrackSegment
        {
            get { return _previousTrackSegment; }
            set { base.PreviousTrackSegment = value; CalculateDerivedValues();}
        }

        /// <summary>
        /// the middle point of the circle which defines the curve
        /// </summary>
        [XmlIgnore]
        public Vector2 MiddlePoint { get { return _middlePoint; } }

        private CornerType _type;
        private float _angle, _radius;
        private Vector2 _middlePoint;
        private float _startingAngle; //the angle of the direction vector at the beginning of the curve in polar coordinates to the middlePoint

        /// <summary>
        /// a corner of a track
        /// </summary>
        /// <param name="previousTrackSegment">the previous TrackSegment. Must not be null</param>
        /// <param name="trackWidthEnd">the width of the Curve</param>
        /// <param name="endPoint">The Point where the corner ends</param>
        public Curve([NotNull] TrackSegment previousTrackSegment, float trackWidthEnd, Vector2 endPoint)
            : base(previousTrackSegment, trackWidthEnd, endPoint, null)
        {
            CalculateDerivedValues();
            _endDirection = GetDirectionAtAngle(_angle);
        }

        private Curve() : base(null, 0, null, null) { } //just for Xml-Storing

        /// <summary>
        /// gets the direction vector on the curve at the angle phi which runs from the startPoint
        /// </summary>
        /// <param name="phi">the angle</param>
        /// <returns></returns>
        public Vector2 GetDirectionAtAngle(float phi)
        {
            //this is the derivation of the function GetPointAtAngle
            if (Type == CornerType.RightCorner)
                phi = _startingAngle - phi;
            else
                phi += _startingAngle;
            return _radius*new Vector2(Convert.ToSingle(-Math.Sin(phi)), Convert.ToSingle(Math.Cos(phi)));
        }

        /// <summary>
        /// gets the point on the curve at the angle phi which runs from the startPoint
        /// </summary>
        /// <param name="phi">the angle</param>
        /// <param name="radius">if a radius other than the curves one shall be used</param>
        /// <returns></returns>
        public Vector2 GetPointAtAngle(float phi, float radius = 0)
        {
            if (radius.Equals(0))
                radius = _radius;
            if (Type == CornerType.RightCorner)
                phi = _startingAngle - phi + (float)Math.PI;
            else
                phi += _startingAngle;
            return _middlePoint +
                   radius*new Vector2(Convert.ToSingle(Math.Cos(phi)), Convert.ToSingle(Math.Sin(phi)));
        }

        /// <summary>
        /// calculates values which are defined in the classes which inherits from TrackSegment according to the properties defined in TrackSegment
        /// </summary>
        protected override void CalculateDerivedValues()
        {
            //CornerType
            //c_ = k*a_ + h*b_ = EndPoint-StartPoint
            //if h>0 => right else left
            Vector2 c = EndPoint - PreviousTrackSegment.EndPoint;
            Vector2 a = PreviousTrackSegment.EndDirection;
            Vector2 b = a.Normal();
            if ((a.X * c.Y - c.X) / (b.Y * a.X - b.X - a.Y) > 0)
                _type = CornerType.RightCorner;
            else
                _type = CornerType.LeftCorner;

            if (EndPoint.Equals(PreviousTrackSegment.EndPoint)) //complete circle
            {
                Radius = (TrackWidthEnd + PreviousTrackSegment.TrackWidthEnd) / 2;
                Angle = Convert.ToSingle(2 * Math.PI);
                //set the radius to the middle radius of the trackwidth of this curve and the previous tracksegment
                _middlePoint = PreviousTrackSegment.EndDirection.Normal().Normalize() * Radius +
                               PreviousTrackSegment.EndPoint;
            }
            else
            {
                //calculate middle point
                //the middle point is where the normal vector of the vector between start and endpoint intercects with the normal vector of the starting direction
                Vector2 connectingVector = (EndPoint - PreviousTrackSegment.EndPoint) / 2; //2 to get the middle of the distance
                Vector2 normalVectorMiddle = connectingVector.Normal();
                _middlePoint = normalVectorMiddle.GetIntersectionPoint(PreviousTrackSegment.EndDirection.Normal(),
                    PreviousTrackSegment.EndPoint, PreviousTrackSegment.EndPoint + connectingVector);

                //calculate radius
                _radius = (EndPoint - _middlePoint).Magnitude;
                //calculate angle
                if(Type == CornerType.LeftCorner)
                    _angle = normalVectorMiddle.GetEnclosedAngle(_endPoint - _middlePoint) * 2;
                else
                    _angle = normalVectorMiddle.GetEnclosedAngle(_middlePoint - _endPoint) * 2;
            }

            CalculateStartingAngle();
        }

        /// <summary>
        /// Calculates the angel at the beginning of the Curve. 0 is on the positive x-axis from the perspective of the middle point
        /// </summary>
        private void CalculateStartingAngle()
        {
            //the if clauses are according to the definition of the atan
            float x = _middlePoint.X - PreviousTrackSegment.EndPoint.X;
            float y = _middlePoint.Y - PreviousTrackSegment.EndPoint.Y;
            if (x > 0)
                _startingAngle = Convert.ToSingle(Math.Atan(y / x));
            else if (x.Equals(0))
            {
                if (y > 0)
                    _startingAngle = Convert.ToSingle(Math.Atan(y / x) + Math.PI);
                else if (y.Equals(0f))
                    _startingAngle = 0;
                else
                    _startingAngle = Convert.ToSingle(Math.Atan(y / x) - Math.PI);
            }
            else
            {
                if (y > 0)
                    _startingAngle = Convert.ToSingle(Math.PI / 2);
                else if (y.Equals(0f))
                    _startingAngle = 0;
                else
                    _startingAngle = -Convert.ToSingle(Math.PI / 2);
            }
        }

        /// <summary>
        /// calculates properties which are defined in TrackSegment according to the values in a class which inherits from TrackSegment
        /// </summary>
        protected override void CalculateBaseValues()
        {
            _endDirection = GetDirectionAtAngle(_angle);
            _endPoint = GetPointAtAngle(_angle);
        }

        /// <summary>
        /// invoked by the TrackSegmentChanged event of the PreviousTrackSegment
        /// </summary>
        protected override void PreviousTrackSegmentChanged()
        {
            if (Type == CornerType.RightCorner)
                _middlePoint = PreviousTrackSegment.EndPoint +
                               PreviousTrackSegment.EndDirection.Normal().Normalize()*Radius;
            else
                _middlePoint = PreviousTrackSegment.EndPoint -
                               PreviousTrackSegment.EndDirection.Normal().Normalize() * Radius;
            CalculateStartingAngle();
            CalculateBaseValues();
            CallTrackSegmentChangedEvent();
        }
    }
}
