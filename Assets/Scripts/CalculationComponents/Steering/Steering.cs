using System;
using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents
{
    /// <summary>
    /// calculates the steering of the car
    /// </summary>
    public class Steering : ICalculationComponent
    {
        /// <summary>
        /// The angle of the right wheel according to the steering angle (both in radiant)
        /// </summary>
        [Setting("Wheelangle right / Steeringangle (radiant)")]
        public Spline RightWheelAngle { get; set; }
        /// <summary>
        /// The angle of the left wheel according to the steering angle (both in radiant)
        /// </summary>
        [Setting("Wheelangle left / Steeringangle (radiant)")]
        public Spline LeftWheelAngle { get; set; }
        /// <summary>
        /// the maximum steering angle (in radiant)
        /// </summary>
        [Setting("Maximum steeringangle +- (radiant)")]
        public float MaxSteeringAngle { get; set; }
        
        private readonly SteeringOutput _actualCalculation;
        
        /// <summary>
        /// calculates the steering of the car
        /// </summary>
        public Steering()
        {
            _actualCalculation = new SteeringOutput();
            LeftWheelAngle = new Spline(new[] { -Math.PI, Math.PI }, new[] { Math.PI / 4, -Math.PI / 4 });
            RightWheelAngle = new Spline(new[] { -Math.PI, Math.PI }, new[] { Math.PI / 4, -Math.PI / 4 });
            RightWheelAngle.NameX = "Steeringangle (rad)";
            RightWheelAngle.NameY = "Wheelangle right (rad)";
            LeftWheelAngle.NameY = "wheelangle left (rad)";
            LeftWheelAngle.NameX = "Steeringangle (rad)";
            MaxSteeringAngle = 1;
        }

        /// <summary>
        /// Calculates the steering according to the input data
        /// </summary>
        public void Calculate()
        {
            //calculate the angle for each wheel
            _actualCalculation.WheelAngleLeft =
                Convert.ToSingle(LeftWheelAngle.Interpolate(InputData.UsedInputData.Steering * MaxSteeringAngle));
            _actualCalculation.WheelAngleRight =
                Convert.ToSingle(RightWheelAngle.Interpolate(InputData.UsedInputData.Steering * MaxSteeringAngle));

            float wheelAngleMid = (_actualCalculation.WheelAngleLeft + _actualCalculation.WheelAngleRight)/2;
            if (wheelAngleMid.Equals(0f)) //straight ahead
            {
                _actualCalculation.RadiusFrontAxis = float.PositiveInfinity;
                _actualCalculation.RadiusRearAxis = float.PositiveInfinity;
            }
            else
            {
                _actualCalculation.RadiusFrontAxis = CalculationController.Instance.OverallCar.Wheelbase /
                                                     (float)Math.Sin(Math.Abs(wheelAngleMid));
                _actualCalculation.RadiusRearAxis = CalculationController.Instance.OverallCar.Wheelbase /
                                                     (float)Math.Tan(Math.Abs(wheelAngleMid));
            }

            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if running
        /// </summary>
        public void StopCalculation()
        {
            //the calculation is too short as it is worthwhile to implement it
        }

        /// <summary>
        /// stores the result of the calculation to the SteeringOutput class
        /// </summary>
        public void StoreResult()
        {
            SteeringOutput.LastCalculation.WheelAngleLeft = _actualCalculation.WheelAngleLeft;
            SteeringOutput.LastCalculation.WheelAngleRight = _actualCalculation.WheelAngleRight;
            SteeringOutput.LastCalculation.RadiusFrontAxis = _actualCalculation.RadiusFrontAxis;
            SteeringOutput.LastCalculation.RadiusRearAxis = _actualCalculation.RadiusRearAxis;
        }

        /// <summary>
        /// this function is not needed for the steering
        /// </summary>
        public void CalculateBackwards()
        {
            //here is nothing to calculate backwards
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
