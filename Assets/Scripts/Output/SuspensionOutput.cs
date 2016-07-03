using CalculationComponents.Enums;

namespace Output
{
    public class SuspensionOutput
    {
        public static SuspensionOutput[] LastCalculations
        {
            get
            {
                if (_lastCalculations == null)
                {
                    _lastCalculations = new SuspensionOutput[4];
                    for (int i = 0; i < 4; i++)
                        _lastCalculations[i] = new SuspensionOutput();
                }
                return _lastCalculations;
            }
        }
        private static SuspensionOutput[] _lastCalculations;

        public float Torque { get; set; }
        public float WheelLoad { get; set; }
        public float WheelAngle { get; set; }

        public static SuspensionOutput GetLastCalculation(Wheels wheel)
        {
            return LastCalculations[(int)wheel];
        }

        public static void SetLastCalculation(Wheels wheel, SuspensionOutput output)
        {
            int index = (int)wheel;
            LastCalculations[index].Torque = output.Torque;
            LastCalculations[index].WheelAngle = output.WheelAngle;
            LastCalculations[index].WheelLoad = output.WheelLoad;
        }
    }
}
