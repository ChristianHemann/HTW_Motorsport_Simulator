using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;
namespace Output
{
  public  class SteeringOutput
    {
        private static SteeringOutput _lastCalculation;
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

        public float WheelAngleLeft { get; set; }
        public float WheelAngleRight { get; set; }
        public float RadiusFrontAxis { get; set; }
        public float RadiusRearAxis { get; set; }
    }
}
