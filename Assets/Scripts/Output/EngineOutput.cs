
namespace Output
{
    /// <summary>
    /// stores the calculation results of the engine
    /// </summary>
    public class EngineOutput
    {
        /// <summary>
        /// used to store the calculation results of the last calculation step
        /// </summary>
        public static EngineOutput LastCalculation {
            get
            {
                if (_lastCalculation == null)
                    _lastCalculation = new EngineOutput();
                return _lastCalculation;
            }
            set { _lastCalculation = value; }
        }
        private static EngineOutput _lastCalculation;

        /// <summary>
        /// the output torque of the engine
        /// </summary>
        public float Torque { get; set; }
        /// <summary>
        /// the revolution speed of the gearbox in rounds per minute
        /// </summary>
        public float Rpm { get; set; }
    }
}
