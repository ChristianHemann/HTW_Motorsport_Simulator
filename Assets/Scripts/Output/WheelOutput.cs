using ImportantClasses.Enums;
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
        public float LongitudinalAccelerationForce { get; set; }
        public float LongitudinalDecelerationForce { get; set; }
        public float LateralAcceleration { get; set; }
        public int Slip { get; set; }

        public WheelOutput()
        {
            Direction = new Vector2(0, 0);
            LongitudinalAccelerationForce = 0;
            LongitudinalDecelerationForce = 0;
            LateralAcceleration = 0;
            Slip = 0;
        }

        public static WheelOutput GetWheelOutput(Wheels wheel)
        {
            return LastCalculations[(int) wheel];
        }

        public static void SetWheelOutput(Wheels wheel, WheelOutput output)
        {
            int index = (int) wheel;
            LastCalculations[index].LongitudinalAccelerationForce = output.LongitudinalAccelerationForce;
            LastCalculations[index].LongitudinalDecelerationForce = output.LongitudinalDecelerationForce;
            LastCalculations[index].LateralAcceleration = output.LateralAcceleration;
            LastCalculations[index].Slip = output.Slip;
            LastCalculations[index].Direction = output.Direction;
        }
    }
}
