using CalculationComponents.WheelComponents;
using ImportantClasses;

namespace CalculationComponents
{
    /// <summary>
    /// all the wheels of the vehicle
    /// </summary>
    public class Wheels : ICalculationComponent
    {
        /// <summary>
        /// the friction coefficient of all wheels (unitless)
        /// </summary>
        [Setting("Friction coefficient (unitless)")]
        public float FrictionCoefficient { get; set; }
        /// <summary>
        /// the polar area moment of inertia per wheel(m^4)
        /// </summary>
        [Setting("Polar area moment of inertia per wheel(m^4)")]
        public float InertiaTorque { get; set; }
        /// <summary>
        /// the weight of one wheel (kg)
        /// </summary>
        [Setting("Weight per wheel (kg)")]
        public float Weight { get; set; }
        /// <summary>
        /// the diameter of a wheel (m)
        /// </summary>
        [Setting("Diameter (m)")]
        public float Diameter { get; set; }

        private readonly Wheel[] _wheels; //each wheel
        
        /// <summary>
        /// all the wheels of the vehicle
        /// </summary>
        public Wheels()
        {
            _wheels = new Wheel[4];
            for(int i = 0; i < 4; i++)
            {
                _wheels[i] = new Wheel((ImportantClasses.Enums.Wheels)i);
            }
            Weight = 1;
            Diameter = 0.5f;
            InertiaTorque = 1;
            FrictionCoefficient = 0.9f;
        }

        /// <summary>
        /// calculates all wheels
        /// </summary>
        public void Calculate()
        {
            foreach (Wheel wheel in _wheels)
            {
                wheel.Calculate();
            }
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if running
        /// </summary>
        public void StopCalculation()
        {
            foreach (Wheel wheel in _wheels)
            {
                wheel.StopCalculation();
            }
        }

        /// <summary>
        /// stores the calculation result of each wheel
        /// </summary>
        public void StoreResult()
        {
            foreach (Wheel wheel in _wheels)
            {
                wheel.StoreResult();
            }
        }

        /// <summary>
        /// calls the CalculateBackwards function of each wheel
        /// </summary>
        public void CalculateBackwards()
        {
            foreach (Wheel wheel in _wheels)
            {
                wheel.CalculateBackwards();
            }
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
