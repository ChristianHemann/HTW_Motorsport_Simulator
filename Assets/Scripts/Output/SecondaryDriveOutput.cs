using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
   public class SecondaryDriveOutput
    {
        private static SecondaryDriveOutput _lastCalculation;
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

        public float Torque { get; set; }
        public float Rpm { get; set; }
    }
}
