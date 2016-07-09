using System;
using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents
{
    /// <summary>
    /// calculates the chassis and general properties of the car
    /// </summary>
    public class OverallCar : ICalculationComponent
    {
        /// <summary>
        /// The wheelbase of the car (m)
        /// </summary>
        [Setting("Wheelbase (m)")]
        public float Wheelbase { get; set; }
        /// <summary>
        /// The weight of the complete car (kg)
        /// </summary>
        [Setting("Weight (kg)")]
        public float Weight { get; set; }
        /// <summary>
        /// The trackwidth of the car (m)
        /// </summary>
        [Setting("Track width (m)")]
        public float TrackWidth { get; set; }
        /// <summary>
        /// the air drag of the car in Force per speed (N/(m/s))
        /// </summary>
        [Setting("Drag (N/(m/s))")]
        public Spline Drag { get; set; }

        private readonly OverallCarOutput _actualCalculation;

        /// <summary>
        /// calculates the chassis and general properties of the car
        /// </summary>
        public OverallCar()
        {
            _actualCalculation = new OverallCarOutput();
            Wheelbase = 1;
            Weight = 100;
            TrackWidth = 1;
            Drag = new Spline(new[] { -100d, 100d }, new[] { -10d, 10d });
        }

        /// <summary>
        /// Calculates the overall car according to the results of all the other classes
        /// </summary>
        public void Calculate()
        {
            Vector3 totalLongitudinalAcForce = new Vector3(); //saves the longitudal acceleration force of all wheels
            Vector3 totalLongitudinalDeForce = new Vector3(); //saves the longitudal deceleration force of all wheels
            Vector3 totalLateralAcceleration = new Vector3(); //saves the lateral acceleration of all wheels

            //accumulate the forces of all wheels
            for (int i = 0; i < 4; i++) 
            {
                WheelOutput wheelOutput = WheelOutput.GetWheelOutput((ImportantClasses.Enums.Wheels)i);
                totalLongitudinalAcForce += wheelOutput.LongitudinalAccelerationForce * wheelOutput.Direction;
                totalLongitudinalDeForce += wheelOutput.LongitudinalDecelerationForce * wheelOutput.Direction;
                if (!float.IsNaN(wheelOutput.LateralAcceleration))
                    totalLateralAcceleration += wheelOutput.LateralAcceleration*
                                                wheelOutput.Direction.Rotate(-Math.PI/2);
            }
            //calculate the change of the speed due to the acceleration, the air drag and the deceleration from the brake
            Vector3 velocityChangeAc = (totalLongitudinalAcForce / Weight + totalLateralAcceleration) * CalculationController.Instance.Duration;
            Vector3 velocityChangeDrag = Drag.Interpolate(OverallCarOutput.LastCalculation.Speed)*
                                         OverallCarOutput.LastCalculation.Direction*
                                         CalculationController.Instance.Duration;
            Vector3 velocityChangeDe = totalLongitudinalDeForce * CalculationController.Instance.Duration / Weight;

            //calculate the speed which would the car have if the brake is not pushed
            _actualCalculation.Speed =
                (OverallCarOutput.LastCalculation.Speed*OverallCarOutput.LastCalculation.Direction +
                 ((Vector2) velocityChangeAc).Projection((Vector2) OverallCarOutput.LastCalculation.Direction) -
                 velocityChangeDrag).Magnitude;

            //calculate the driving direction of the car
            Vector3 directionBuffer = 
                (OverallCarOutput.LastCalculation.Speed*OverallCarOutput.LastCalculation.Direction + velocityChangeAc -
                 velocityChangeDrag);
            if(directionBuffer.Magnitude>1e-3f) //change the direction just if the car is moving. Otherwise the direction can get lost.
                _actualCalculation.Direction = directionBuffer.Normalize();

            //add the deceleration of the brake and make sure that it cannot accelerate the car backwards
            if (velocityChangeDe.Magnitude > Math.Abs(_actualCalculation.Speed))
            {
                _actualCalculation.Speed = 0;
            }
            else if (_actualCalculation.Speed > 0)
            {
                _actualCalculation.Speed -= velocityChangeDe.Magnitude;
            }
            else
            {
                _actualCalculation.Speed += velocityChangeDe.Magnitude;
            }

            //calculates the new position of the car
            _actualCalculation.Position = OverallCarOutput.LastCalculation.Position +
                                          _actualCalculation.Speed * CalculationController.Instance.Duration * _actualCalculation.Direction;

            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if running
        /// </summary>
        public void StopCalculation()
        {
            //actually it is not implemented
        }

        /// <summary>
        /// stores the results of the latest calculation to the OverallCarOutput class
        /// </summary>
        public void StoreResult()
        {
            OverallCarOutput.LastCalculation.Speed = _actualCalculation.Speed;
            OverallCarOutput.LastCalculation.Position = _actualCalculation.Position;
            OverallCarOutput.LastCalculation.Direction = _actualCalculation.Direction;
        }

        /// <summary>
        /// This function is not necessary for the overall car
        /// </summary>
        public void CalculateBackwards()
        {
            //actually here is nothing to do
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
