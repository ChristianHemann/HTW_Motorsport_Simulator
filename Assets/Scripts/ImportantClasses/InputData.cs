
using CalculationComponents;

namespace ImportantClasses
{
    /// <summary>
    /// saves the current input data to have the same inputs during the calculation procedure
    /// </summary>
    public class InputData
    {
        /// <summary>
        /// Store the actual InputData for using in the next calculation step
        /// </summary>
        public static InputData ActualInputData;

        /// <summary>
        /// The used InputData in the actual Calculation step. Prevents from changing data while calculation
        /// </summary>
        public static InputData UsedInputData;

        /// <summary>
        /// The position of the gas pedal. value from 0 to 1
        /// </summary>
        public float AccelerationPedal { get; set; }

        /// <summary>
        /// The positon of the brake pedal. value from 0 to 1
        /// </summary>
        public float BrakePedal { get; set; }

        /// <summary>
        /// the position of the steering wheel. value between -1 and 1. multiply with the maximum steeering angle to get the real steering angle
        /// </summary>
        public float Steering { get; set; }

        /// <summary>
        /// determines in which gear the car is
        /// </summary>
        public byte Gear
        {
            get { return _gear; }
            set { _gear = value; }
        }
        private byte _gear;

        /// <summary>
        /// saves the current input data to have the same inputs during the calculation procedure
        /// </summary>
        /// <param name="accelerationPedal">the position of the gas pedal. value from 0 to 1</param>
        /// <param name="brakePedal">the position of the brake pedal. Value from 0 to 1</param>
        /// <param name="steering">the position of the steering wheel. value between -1 and 1. multiply with the maximum steeering angle to get the real steering angle</param>
        /// <param name="gear">determines in which gear the car is</param>
        public InputData(float accelerationPedal, float brakePedal, float steering, byte gear)
        {
            AccelerationPedal = accelerationPedal;
            BrakePedal = brakePedal;
            Steering = steering;
            Gear = gear;
        }
    }
}
