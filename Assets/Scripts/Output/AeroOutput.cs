using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
    /// <summary>
    /// Stores calculation results of the aerodynamic
    /// </summary>
   public class AeroOutput
    {
        /// <summary>
        /// the calculation results of the last calculation step
        /// </summary>
        public static AeroOutput LastCalculation
        {
            get
            {
                if (_lastCalculation == null)
                    _lastCalculation = new AeroOutput();
                return _lastCalculation;
            }
            set { _lastCalculation = value; }
        }
        private static volatile AeroOutput _lastCalculation;

        /// <summary>
        /// the downforce of the aerodynamic
        /// </summary>
        public float Downforce { get; set; }
        /// <summary>
        /// the air drag of the aerodynamic
        /// </summary>
        public float Drag { get; set; }

    }

}
