using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;
namespace Output
{
    /// <summary>
    /// Stores calculation results of the steering
    /// </summary>
    public class SteeringOutput
    {
        /// <summary>
        /// the calculation results of the last calculation step
        /// </summary>
        public static SteeringOutput LastCalculation
        {
            get
            {
                if (_lastCalculation == null)
                    _lastCalculation = new SteeringOutput();
                return _lastCalculation;
            }
            set { _lastCalculation = value; }
        }
        private static volatile SteeringOutput _lastCalculation;

        /// <summary>
        /// the angle which the left wheel is turned relative to the car on the z-axis
        /// </summary>
        public float WheelAngleLeft { get; set; }
        /// <summary>
        /// the angle which the right wheel is turned relative to the car on the z-axis
        /// </summary>
        public float WheelAngleRight { get; set; }
        /// <summary>
        /// the middle radius which the wheels on the front would take
        /// </summary>
        public float RadiusFrontAxis { get; set; }
        /// <summary>
        /// the middle radius which the wheels on the rear would take
        /// </summary>
        public float RadiusRearAxis { get; set; }
    }
}
