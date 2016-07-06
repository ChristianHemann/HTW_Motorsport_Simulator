using System;
using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents
{
    public class Steering : ICalculationComponent
    {
        [Setting("Wheelangle right / Steeringangle (radiant)")]
        public Spline RightWheelAngle { get; set; }
        [Setting("Wheelangle left / Steeringangle (radiant)")]
        public Spline LeftWheelAngle { get; set; }
        [Setting("Maximum steeringangle +- (radiant)")]
        public float MaxSteeringAngle { get; set; }
        
        private SteeringOutput _actualCalculation;

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

        public void Calculate()
        {
            _actualCalculation.WheelAngleLeft =
                Convert.ToSingle(LeftWheelAngle.Interpolate(InputData.UsedInputData.Steering * MaxSteeringAngle));
            _actualCalculation.WheelAngleRight =
                Convert.ToSingle(RightWheelAngle.Interpolate(InputData.UsedInputData.Steering * MaxSteeringAngle));

            float wheelAngleMid = (_actualCalculation.WheelAngleLeft + _actualCalculation.WheelAngleRight)/2;
            if (wheelAngleMid.Equals(0f))
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

        public void StopCalculation()
        {
            //the calculation is too short as it is worthwhile to implement it
        }

        public void StoreResult()
        {
            SteeringOutput.LastCalculation.WheelAngleLeft = _actualCalculation.WheelAngleLeft;
            SteeringOutput.LastCalculation.WheelAngleRight = _actualCalculation.WheelAngleRight;
            SteeringOutput.LastCalculation.RadiusFrontAxis = _actualCalculation.RadiusFrontAxis;
            SteeringOutput.LastCalculation.RadiusRearAxis = _actualCalculation.RadiusRearAxis;
        }

        public void CalculateBackwards()
        {
            //here is nothing to calculate backwards
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
