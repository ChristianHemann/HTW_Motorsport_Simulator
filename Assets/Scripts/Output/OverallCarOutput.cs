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

        public OverallCarOutput()
        {
            Speed = new Vector3();
            Position = new Vector3();
        }
        
        /// <summary>
        /// the speed of the car in m/s
        /// </summary>
        public Vector3 Speed { get; set; }
        /// <summary>
        /// the movement direction of the car
        /// </summary>
        public Vector3 Position { get; set; }
    }
}
