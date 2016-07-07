using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents.WheelComponents
{
    /// <summary>
    /// one wheel of the vehicle
    /// </summary>
    public class Wheel : ICalculationComponent
    {
        private readonly ImportantClasses.Enums.Wheels _wheel; //determines which wheel is represented
        private readonly WheelOutput _actualCalculation;

        /// <summary>
        /// one wheel of the vehicle
        /// </summary>
        /// <param name="wheel">which wheel?</param>
        public Wheel(ImportantClasses.Enums.Wheels wheel)
        {
            _actualCalculation = new WheelOutput();
            _wheel = wheel;
        }

        /// <summary>
        /// calculates a single wheel
        /// </summary>
        public void Calculate()
        {
            SuspensionOutput suspensionCalculation = SuspensionOutput.GetLastCalculation(_wheel); //get the outputs of the suspension for this wheel
            //calculates the half width of the car, so that it is not needed to calculate a couple of times
            float halfTrackWidth = CalculationController.Instance.OverallCar.TrackWidth / 2;

            //force = torque/radius of a wheel
            _actualCalculation.LongitudinalAccelerationForce = suspensionCalculation.AccelerationTorque /
                                                   (CalculationController.Instance.Wheels.Diameter / 2);
            if ((int)_wheel < 2) //front axis
                _actualCalculation.LongitudinalDecelerationForce = BrakeOutput.LastCalculation.BrakeMomentFront / 2;
            else //rear axis
                _actualCalculation.LongitudinalDecelerationForce = BrakeOutput.LastCalculation.BrakeMomentRear / 2;

            //calculate the drivingRadius and the lateral acceleration
            float drivingRadius; //the radius which the wheel will take in a curve
            if (suspensionCalculation.WheelAngle < 0) //drive right
            {
                drivingRadius = ((int)_wheel < 2
                    ? SteeringOutput.LastCalculation.RadiusFrontAxis
                    : SteeringOutput.LastCalculation.RadiusRearAxis) +
                                (((int)_wheel) % 2 == 0 ? halfTrackWidth : -halfTrackWidth);
                _actualCalculation.LateralAcceleration = OverallCarOutput.LastCalculation.Speed *
                                                         OverallCarOutput.LastCalculation.Speed / drivingRadius;
            }
            else if (suspensionCalculation.WheelAngle > 0) //drive left
            {
                drivingRadius = ((int)_wheel < 2
                    ? SteeringOutput.LastCalculation.RadiusFrontAxis
                    : SteeringOutput.LastCalculation.RadiusRearAxis) +
                                (((int)_wheel) % 2 == 0 ? -halfTrackWidth : halfTrackWidth);
                _actualCalculation.LateralAcceleration = -OverallCarOutput.LastCalculation.Speed *
                                                         OverallCarOutput.LastCalculation.Speed / drivingRadius;
            }
            else //drive straight ahead
            {
                drivingRadius = float.PositiveInfinity;
                _actualCalculation.LateralAcceleration = 0;
            }
            //calculate the direction in which the wheel would like to drive
            _actualCalculation.Direction = ((Vector2)OverallCarOutput.LastCalculation.Direction).Rotate(suspensionCalculation.WheelAngle);

            _actualCalculation.Slip = 0; //actually the calculation is not ready; the slip is allways zero and the maximum force is infinity
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// this function is not needed for a single Wheel
        /// </summary>
        public void CalculateBackwards()
        {
            //actually here is nothing to calculate
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if running
        /// </summary>
        public void StopCalculation()
        {
            //actually the calculation is too short
        }

        /// <summary>
        /// stores the calculation results of the wheel
        /// </summary>
        public void StoreResult()
        {
            WheelOutput.SetWheelOutput(_wheel, _actualCalculation);
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
