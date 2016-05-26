using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
    public class GearBoxOutput
    {
        public static GearBoxOutput _lastCalculation;

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

        public float torque { get; set; }
        public float rpm { get; set; } //rounds per minute
    }
}
