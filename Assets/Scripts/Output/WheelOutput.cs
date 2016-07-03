using CalculationComponents.Enums;

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

        private static WheelOutput[] _lastCalculations;

        public float ForceX { get; set; }
        public float ForceY { get; set; }
        public int Slip { get; set; }

        public static WheelOutput GetWheelOutput(Wheels wheel)
        {
            return _lastCalculations[(int) wheel];
        }

        public static void SetWheelOutput(Wheels wheel, WheelOutput output)
        {
            int index = (int) wheel;
            _lastCalculations[index].ForceX = output.ForceX;
            _lastCalculations[index].ForceY = output.ForceY;
            _lastCalculations[index].Slip = output.Slip;
        }
    }
}
