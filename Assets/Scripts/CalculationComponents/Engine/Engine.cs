using System;
using ImportantClasses;
using Output;

namespace CalculationComponents
{
    /// <summary>
    /// calculates an engine
    /// </summary>
    public class Engine : ICalculationComponent
    {
        [Setting("Characteristic Line")]
        public Spline CharacteristicLine { get; set; }

        [Setting("max. rpm")]
        public int MaxRpm { get; set; }

        private EngineOutput actualCalculation; //the result of the actual done calculation

        /// <summary>
        /// calculates an engine. Will be initialized with the standard values
        /// </summary>
        public Engine()
        {
            CharacteristicLine = new Spline(new[] {0.0, 10000.0}, new[] {0.0, 100.0});
            CharacteristicLine.NameX = "rounds per minute";
            CharacteristicLine.NameY = "torque";
            actualCalculation = new EngineOutput();
        }

        /// <summary>
        /// calculate the engineOutputs with the actual inputs
        /// </summary>
        public void Calculate()
        {
            actualCalculation.torque = (float) CharacteristicLine.Interpolate(EngineOutput.LastCalculation.rpm) * InputData.UsedInputData.AccelerationPedal;
            if(OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if it necessary to abort it
        /// </summary>
        public void StopCalculation()
        {
            //the function is so small, that it makes no sense
        }

        /// <summary>
        /// stores the result of the calculation to the EngineOutput class
        /// </summary>
        public void StoreResult()
        {
            EngineOutput.LastCalculation.torque = actualCalculation.torque;
            //the rpm will be calculated by the car when its speed is known
        }

        /// <summary>
        /// Called when all Calculations are ready to calculate the engine rpm
        /// </summary>
        public void CalculateBackwards()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
