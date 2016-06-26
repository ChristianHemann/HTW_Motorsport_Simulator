using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
   public class AeroOutput
    {
        private static AeroOutput _lastCalculation;
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

        public float Downforce { get; set; }
        public float Drag { get; set; }

    }

}
