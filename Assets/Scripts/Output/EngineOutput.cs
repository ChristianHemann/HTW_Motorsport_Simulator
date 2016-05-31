using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;
namespace Output
{
    public class EngineOutput
    {
        private static EngineOutput _lastCalculation;
        public static EngineOutput LastCalculation {
            get
            {
                if (_lastCalculation == null)
                    _lastCalculation = new EngineOutput();
                return _lastCalculation;
            }
            set { _lastCalculation = value; }
        }
        
        public float torque { get; set; }
        public float rpm { get; set; } //rounds per minute
    }
}
