using ImportantClasses.Enums;
using ImportantClasses;

namespace Output
{
    /// <summary>
    /// Stores calculation results of the wheels
    /// </summary>
    public class WheelOutput
    {
        /// <summary>
        /// the calculation results of the last calculation step
        /// </summary>
        public static WheelOutput[] LastCalculations
        {
            get
            {
                if (_lastCalculations == null)
                {
                    _lastCalculations = new WheelOutput[4];
                    for(int i = 0;i<4;i++)
                        _lastCalculations[i] = new WheelOutput();
                }
                return _lastCalculations;
            }
        }
        private static volatile WheelOutput[] _lastCalculations;

        /// <summary>
        /// The direction in which the wheel wants to drive in absolute coordinates
        /// </summary>
        public Vector2 Direction { get; set; }
        /// <summary>
        /// the longitudinal force which accelerates the wheel
        /// </summary>
        public float LongitudinalAccelerationForce { get; set; }
        /// <summary>
        /// the longitudinal force which decelerates the wheel
        /// </summary>
        public float LongitudinalDecelerationForce { get; set; }
        /// <summary>
        /// the lateral force which the wheel perform on the car
        /// </summary>
        public float LateralAcceleration { get; set; }
        /// <summary>
        /// the slip factor of the wheel
        /// </summary>
        public int Slip { get; set; }

        /// <summary>
        /// Stores calculation results of the wheels
        /// </summary>
        public WheelOutput()
        {
            Direction = new Vector2(0, 0);
            LongitudinalAccelerationForce = 0;
            LongitudinalDecelerationForce = 0;
            LateralAcceleration = 0;
            Slip = 0;
        }

        /// <summary>
        /// gets the last calculation results of the specified wheel
        /// </summary>
        /// <param name="wheel">the destinated wheel</param>
        /// <returns>the last calculation result</returns>
        public static WheelOutput GetWheelOutput(Wheels wheel)
        {
            return LastCalculations[(int) wheel];
        }

        /// <summary>
        /// sets the last calculation results of the specified wheel
        /// </summary>
        /// <param name="wheel">the destinated wheel</param>
        /// <param name="output">the calculation result to set</param>
        public static void SetWheelOutput(Wheels wheel, WheelOutput output)
        {
            int index = (int) wheel;
            LastCalculations[index].LongitudinalAccelerationForce = output.LongitudinalAccelerationForce;
            LastCalculations[index].LongitudinalDecelerationForce = output.LongitudinalDecelerationForce;
            LastCalculations[index].LateralAcceleration = output.LateralAcceleration;
            LastCalculations[index].Slip = output.Slip;
            LastCalculations[index].Direction = output.Direction;
        }
    }
}
