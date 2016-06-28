using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
    /// <summary>
    /// stores the calculation results of the secondary drive
    /// </summary>
   public class SecondaryDriveOutput
    {
        /// <summary>
        /// used to store the calculation results of the last calculation step
        /// </summary>
        public static SecondaryDriveOutput LastCalculation
        {
            get
            {
                if (_lastCalculation == null)
                    _lastCalculation = new SecondaryDriveOutput();
                return _lastCalculation;
            }
            set { _lastCalculation = value; }
        }
        private static SecondaryDriveOutput _lastCalculation;

        /// <summary>
        /// the output torque of the secondary drive
        /// </summary>
        public float Torque { get; set; }
        /// <summary>
        /// the revolution speed of the secondary drive in rounds per minute
        /// </summary>
        public float Rpm { get; set; }
    }
}
