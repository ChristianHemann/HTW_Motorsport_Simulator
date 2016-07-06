using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents
{
    public class OverallCar : ICalculationComponent
    {
        [Setting("Wheelbase (m)")]
        public float Wheelbase { get; set; }
        [Setting("Weight (kg)")]
        public float Weight { get; set; }
        [Setting("Track width (m)")]
        public float TrackWidth { get; set; }
        [Setting("Drag (N/(m/s))")]
        public Spline Drag { get; set; }

        private readonly OverallCarOutput _actualCalculation;

        public OverallCar()
        {
            _actualCalculation = new OverallCarOutput();
            Wheelbase = 1;
            Weight = 100;
            TrackWidth = 1;
            Drag = new Spline(new[] {0, 100d}, new[] {0d, 0d});
        }

        public void Calculate()
        {
            Vector3 totalLongitudinalForce = new Vector3();
            Vector3 totalLateralAcceleration = new Vector3();
            for (int i = 0; i < 4; i++)
            {
                WheelOutput wheelOutput = WheelOutput.GetWheelOutput((Enums.Wheels)i);
                if(!float.IsNaN(wheelOutput.LongitudinalForce))
                    totalLongitudinalForce += wheelOutput.LongitudinalForce * wheelOutput.Direction;
                if(!float.IsNaN(wheelOutput.LateralAcceleration))
                    totalLateralAcceleration += wheelOutput.LateralAcceleration * wheelOutput.Direction;
            }
            Vector3 totalAcceleration = totalLongitudinalForce/Weight + totalLateralAcceleration -
                                        OverallCarOutput.LastCalculation.Speed.Normalize()*
                                        Drag.Interpolate(OverallCarOutput.LastCalculation.Speed.Magnitude);
            Vector3 velocityChange = totalAcceleration * CalculationController.Instance.Duration;
            _actualCalculation.Speed = OverallCarOutput.LastCalculation.Speed + velocityChange;
            _actualCalculation.Position = OverallCarOutput.LastCalculation.Position +
                                          _actualCalculation.Speed*CalculationController.Instance.Duration;
        }

        public void StopCalculation()
        {
            //actually the calculation is too short
        }

        public void StoreResult()
        {
            OverallCarOutput.LastCalculation.Speed = _actualCalculation.Speed;
            OverallCarOutput.LastCalculation.Position = _actualCalculation.Position;
        }

        public void CalculateBackwards()
        {
            //actually here is nothing to do
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
