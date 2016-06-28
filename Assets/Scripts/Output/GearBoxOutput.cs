using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
    /// <summary>
    /// stores the calculation results of the gearbox
    /// </summary>
    public class GearBoxOutput
    {
        /// <summary>
        /// used to store the calculation results of the last calculation step
        /// </summary>
        public static GearBoxOutput LastCalculation
        {
            get
            {
                if(_lastCalculation == null)
                    _lastCalculation = new GearBoxOutput();
                return _lastCalculation;
            }
            set { _lastCalculation = value; }
        }
        private static GearBoxOutput _lastCalculation;

        /// <summary>
        /// the output torque of the gearbox
        /// </summary>
        public float Torque { get; set; }
        /// <summary>
        /// the revolution speed of the gearbox in rounds per minute
        /// </summary>
        public float Rpm { get; set; }
    }
}
