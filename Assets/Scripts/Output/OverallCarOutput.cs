using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;
using ImportantClasses;

namespace Output
{
    /// <summary>
    /// saves the calculation results of the overall car
    /// </summary>
    public class OverallCarOutput
    {
        /// <summary>
        /// the calculation results of the last calculation step of the overall car
        /// </summary>
        public static OverallCarOutput LastCalculation
        {
            get
            {
                if (_lastCalculation == null)
                    _lastCalculation = new OverallCarOutput();
                return _lastCalculation;
            }
            set { _lastCalculation = value; }
        }
        private static volatile OverallCarOutput _lastCalculation;

        /// <summary>
        /// the acceleration with its direction of the car in m/s^2
        /// </summary>
        public Vector3 Acceleration { get; set; }
        /// <summary>
        /// the speed of the car in m/s
        /// </summary>
        public float Speed { get; set; }
        /// <summary>
        /// the movement direction of the car
        /// </summary>
        public Vector3 Direction { get; set; }
        /// <summary>
        /// the calculated position of the car
        /// </summary>
        public Vector3 Position { get; set; }
    }
}
