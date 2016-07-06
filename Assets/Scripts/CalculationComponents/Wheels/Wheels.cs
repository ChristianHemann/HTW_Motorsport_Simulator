using CalculationComponents.WheelComponents;
using ImportantClasses;

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

        private readonly Wheel[] _wheels;

        public Wheels()
        {
            _wheels = new Wheel[4];
            for(int i = 0; i < 4; i++)
            {
                _wheels[i] = new Wheel((Enums.Wheels)i);
            }
            Weight = 1;
            Diameter = 0.5f;
            InertiaTorque = 1;
            FrictionCoefficient = 0.9f;
        }

        public void Calculate()
        {
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
