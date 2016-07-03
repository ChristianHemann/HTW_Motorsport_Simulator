using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents.WheelComponents
{
    public class Wheel : ICalculationComponent
    {
        public float DrivingRadius = 0;
        private float _maxForce;
        private readonly WheelOutput _actualCalculation;
        private readonly Enums.Wheels _wheel;

        public Wheel(Enums.Wheels wheel)
        {
            _actualCalculation = new WheelOutput();
            _wheel = wheel;
        }

        public void Calculate()
        {
            _maxForce = SuspensionOutput.GetLastCalculation(_wheel).WheelLoad * CalculationController.Instance.Wheels.FrictionCoefficient;



            _actualCalculation.Slip = 0;
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
