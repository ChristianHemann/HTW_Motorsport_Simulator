
namespace Output
{
    public class BrakeOutput
    {
        private static BrakeOutput _lastCalculation;

        public static BrakeOutput LastCalculation
        {
            get
            {
                if (_lastCalculation == null)
                    _lastCalculation = new BrakeOutput();
                return _lastCalculation;
            }
            set { _lastCalculation = value; }
        }

        public float BrakeMomentFront { get; set; }
        public float BrakeMomentRear { get; set; }
    }
}
