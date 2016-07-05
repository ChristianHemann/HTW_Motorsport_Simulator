using CalculationComponents.Enums;
using ImportantClasses;

namespace Output
{
    public class WheelOutput
    {
        public static WheelOutput[] LastCalculations
        {
            get
            {
                if (_lastCalculations == null)
                {
                    _lastCalculations = new WheelOutput[4];
                    for(int i = 0;i<4;i++)
                        _lastCalculations[i] = new WheelOutput();
                }
                return _lastCalculations;
            }
        }

        private static volatile WheelOutput[] _lastCalculations;

        public Vector2 Direction { get; set; }
        public float LongitudinalForce { get; set; }
        public float LateralAcceleration { get; set; }
        public int Slip { get; set; }

        public static WheelOutput GetWheelOutput(Wheels wheel)
        {
            return LastCalculations[(int) wheel];
        }

        public static void SetWheelOutput(Wheels wheel, WheelOutput output)
        {
            int index = (int) wheel;
            LastCalculations[index].LongitudinalForce = output.LongitudinalForce;
            LastCalculations[index].LateralAcceleration = output.LateralAcceleration;
            LastCalculations[index].Slip = output.Slip;
        }
    }
}
