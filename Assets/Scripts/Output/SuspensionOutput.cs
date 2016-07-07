using ImportantClasses.Enums;

namespace Output
{
    /// <summary>
    /// Stores calculation results of the suspension
    /// </summary>
    public class SuspensionOutput
    {
        /// <summary>
        /// the calculation results of the last calculation step
        /// </summary>
        public static SuspensionOutput[] LastCalculations
        {
            get
            {
                if (_lastCalculations == null)
                {
                    _lastCalculations = new SuspensionOutput[4];
                    for (int i = 0; i < 4; i++)
                        _lastCalculations[i] = new SuspensionOutput();
                }
                return _lastCalculations;
            }
        }
        private static volatile SuspensionOutput[] _lastCalculations;

        /// <summary>
        /// the acceleration torque of the wheel
        /// </summary>
        public float AccelerationTorque { get; set; }
        /// <summary>
        /// the force which pulls the wheel down
        /// </summary>
        public float WheelLoad { get; set; }
        /// <summary>
        /// the angle which the wheel is turned relative to the car on the z-axis
        /// </summary>
        public float WheelAngle { get; set; }

        /// <summary>
        /// returns the last calculation result of a wheel
        /// </summary>
        /// <param name="wheel">the destinated wheel</param>
        /// <returns>the last calculation result</returns>
        public static SuspensionOutput GetLastCalculation(Wheels wheel)
        {
            return LastCalculations[(int)wheel];
        }

        /// <summary>
        /// sets the last calculation result of a wheel
        /// </summary>
        /// <param name="wheel">the destinated wheel</param>
        /// <param name="output">the last calculation result of the wheel</param>
        public static void SetLastCalculation(Wheels wheel, SuspensionOutput output)
        {
            int index = (int)wheel;
            LastCalculations[index].AccelerationTorque = output.AccelerationTorque;
            LastCalculations[index].WheelAngle = output.WheelAngle;
            LastCalculations[index].WheelLoad = output.WheelLoad;
        }
    }
}
