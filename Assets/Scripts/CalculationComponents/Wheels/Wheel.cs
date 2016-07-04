using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents.WheelComponents
{
    public class Wheel : ICalculationComponent
    {
        private float _drivingRadius;
        private readonly WheelOutput _actualCalculation;
        private readonly Enums.Wheels _wheel;

        public Wheel(Enums.Wheels wheel)
        {
            _actualCalculation = new WheelOutput();
            _wheel = wheel;
        }

        public void Calculate()
        {
            float halfTrackWidth = CalculationController.Instance.OverallCar.TrackWidth / 2;
            SuspensionOutput suspensionCalculation = SuspensionOutput.GetLastCalculation(_wheel);
            //force = torque/radius
            _actualCalculation.LongitudinalForce = suspensionCalculation.Torque /
                                                   (CalculationController.Instance.Wheels.Diameter / 2);
            float angle = suspensionCalculation.WheelAngle;
            if (angle < 0) //right
            {
                _drivingRadius = ((int)_wheel < 2
                    ? SteeringOutput.LastCalculation.RadiusFrontAxis
                    : SteeringOutput.LastCalculation.RadiusRearAxis) +
                                (((int)_wheel) % 2 == 0 ? halfTrackWidth : -halfTrackWidth);
                _actualCalculation.LateralAcceleration = OverallCarOutput.LastCalculation.Speed *
                                                         OverallCarOutput.LastCalculation.Speed / _drivingRadius;
            }
            else if (angle > 0) //left
            {
                _drivingRadius = ((int)_wheel < 2
                    ? SteeringOutput.LastCalculation.RadiusFrontAxis
                    : SteeringOutput.LastCalculation.RadiusRearAxis) +
                                (((int)_wheel) % 2 == 0 ? -halfTrackWidth : halfTrackWidth);
                _actualCalculation.LateralAcceleration = -OverallCarOutput.LastCalculation.Speed *
                                                         OverallCarOutput.LastCalculation.Speed / _drivingRadius;
            }
            else //straight
            {
                _drivingRadius = float.PositiveInfinity;
                _actualCalculation.LateralAcceleration = 0;
            }
            _actualCalculation.Direction = new Vector2(0, 1f).Turn(angle);

            _actualCalculation.Slip = 0; //actually the calculation is not ready and the slip is allways zero and the maximum force is infinity
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        public void CalculateBackwards()
        {
            //actually here is nothing to calculate
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        public void StopCalculation()
        {
            //actually the calculation is too short
        }

        public void StoreResult()
        {
            WheelOutput.SetWheelOutput(_wheel, _actualCalculation);
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
