using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;
namespace Output
{
  public  class SteeringOutput
    {
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
        private static volatile SteeringOutput _lastCalculation;

        public float WheelAngleLeft { get; set; }
        public float WheelAngleRight { get; set; }
        public float RadiusFrontAxis { get; set; }
        public float RadiusRearAxis { get; set; }
    }
}
