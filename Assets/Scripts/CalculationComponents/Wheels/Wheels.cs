using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents.Enums;
using CalculationComponents.WheelComponents;
using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents
{
    public class Wheels : ICalculationComponent
    {
        [Setting("Friction coefficient (unitless)")]
        public float FrictionCoefficient { get; set; }
        [Setting("Polar area moment of inertia per wheel(m^4)")]
        public float InertiaTorque { get; set; }
        [Setting("Weight per wheel (kg)")]
        public float Weight { get; set; }
        [Setting("Diameter (m)")]
        public float Diameter { get; set; }

        private Wheel[] _wheels;

        public Wheels()
        {
            _wheels = new Wheel[4];
            for(int i = 0; i < 4; i++)
            {
                _wheels[i] = new Wheel((Enums.Wheels)i);
            }
        }

        public void Calculate()
        {
            float angle = (SteeringOutput.LastCalculation.WheelAngleLeft +
                           SteeringOutput.LastCalculation.WheelAngleRight)/2;
            float halfTrackWidth = CalculationController.Instance.OverallCar.TrackWidth/2;

            if (angle > 0)
            {
                _wheels[0].DrivingRadius = SteeringOutput.LastCalculation.RadiusFrontAxis - halfTrackWidth;
                _wheels[1].DrivingRadius = SteeringOutput.LastCalculation.RadiusFrontAxis + halfTrackWidth;
                _wheels[2].DrivingRadius = SteeringOutput.LastCalculation.RadiusRearAxis - halfTrackWidth;
                _wheels[3].DrivingRadius = SteeringOutput.LastCalculation.RadiusRearAxis + halfTrackWidth;
            }
            else if (angle < 0)
            {
                _wheels[0].DrivingRadius = SteeringOutput.LastCalculation.RadiusFrontAxis + halfTrackWidth;
                _wheels[1].DrivingRadius = SteeringOutput.LastCalculation.RadiusFrontAxis - halfTrackWidth;
                _wheels[2].DrivingRadius = SteeringOutput.LastCalculation.RadiusRearAxis + halfTrackWidth;
                _wheels[3].DrivingRadius = SteeringOutput.LastCalculation.RadiusRearAxis - halfTrackWidth;
            }
            else
            {
                _wheels[0].DrivingRadius = 0;
                _wheels[1].DrivingRadius = 0;
                _wheels[2].DrivingRadius = 0;
                _wheels[3].DrivingRadius = 0;
            }

            foreach (Wheel wheel in _wheels)
            {
                wheel.Calculate();
            }
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        public void StopCalculation()
        {
            foreach (Wheel wheel in _wheels)
            {
                wheel.StopCalculation();
            }
        }

        public void StoreResult()
        {
            foreach (Wheel wheel in _wheels)
            {
                wheel.StoreResult();
            }
        }

        public void CalculateBackwards()
        {
            foreach (Wheel wheel in _wheels)
            {
                wheel.CalculateBackwards();
            }
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
