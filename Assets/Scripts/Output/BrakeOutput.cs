
namespace Output
{
    /// <summary>
    /// Stores calculation results of the brake
    /// </summary>
    public class BrakeOutput
    {
        /// <summary>
        /// the calculation results of the last calculation step
        /// </summary>
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

        /// <summary>
        /// the brakemoment of both wheels of the front axis
        /// </summary>
        public float BrakeMomentFront { get; set; }
        /// <summary>
        /// the brakemoment of both wheels of the rear axis
        /// </summary>
        public float BrakeMomentRear { get; set; }
    }
}
