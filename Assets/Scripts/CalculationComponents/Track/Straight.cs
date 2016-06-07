
using ImportantClasses;
using MathNet.Numerics.LinearAlgebra;

namespace CalculationComponents.TrackComponents
{
    public sealed class Straight : TrackSegment
    {
        public float Length
        {
            get { return _lenght; }
            set
            {
                _lenght = value;
                CalculateDerivedValues();
            }
        }

        public override Vector<float> EndDirection
        {
            get { return PreviousTrackSegment.EndDirection; }
        }

        private float _lenght;

        public Straight(TrackSegment previousTrackSegment, float trackWidthStart, float trackWidthEnd, Vector<float> endPoint, Vector<float> endDirection)
            : base(previousTrackSegment, trackWidthStart, trackWidthEnd, endPoint, endDirection)
        {
            CalculateDerivedValues();
        }

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
