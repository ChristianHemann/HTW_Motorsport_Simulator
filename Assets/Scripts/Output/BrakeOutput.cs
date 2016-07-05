
namespace Output
{
    public class BrakeOutput
    {
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
        private static volatile BrakeOutput _lastCalculation;

        public float BrakeMomentFront { get; set; }
        public float BrakeMomentRear { get; set; }
    }
}
