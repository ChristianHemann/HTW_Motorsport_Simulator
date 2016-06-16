
using System.Xml.Serialization;
using ImportantClasses;
using MathNet.Numerics.LinearAlgebra;

namespace CalculationComponents.TrackComponents
{
    public sealed class Straight : TrackSegment
    {
        [XmlIgnore]
        public float Length
        {
            get { return _lenght; }
            set
            {
                _lenght = value;
                CalculateBaseValues();
            }
        }

        [XmlIgnore]
        public override Vector<float> EndDirection
        {
            get { return PreviousTrackSegment.EndDirection; }
        }

        private float _lenght;

        public Straight(TrackSegment previousTrackSegment, float trackWidthEnd, Vector<float> endPoint)
            : base(previousTrackSegment, trackWidthEnd, endPoint, previousTrackSegment.EndDirection)
        {
            CalculateDerivedValues();
        }

        private Straight() : base(null, 0, null, null) { } //just for Xml-Storing

        protected override void CalculateDerivedValues()
        {
            _lenght = (EndPoint - PreviousTrackSegment.EndPoint).GetMagnitude();
        }

        protected override void CalculateBaseValues()
        {
            EndPoint = PreviousTrackSegment.EndPoint + _lenght * EndDirection;
        }
    }
}
