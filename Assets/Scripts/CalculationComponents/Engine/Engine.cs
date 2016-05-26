using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportantClasses;
using Output;

namespace CalculationComponents
{
    public class Engine : ICalculationComponent
    {
        [Setting("Characteristic Line")]
        public Spline characteristicLine { get; set; }

        [Setting("max. rpm")]
        public int maxRpm { get; set; }

        private EngineOutput actualCalculation;
        public Engine()
        {
            characteristicLine = new Spline(new[] {0.0, 10000.0}, new[] {0.0, 100.0});
            characteristicLine.NameX = "rounds per minute";
            characteristicLine.NameY = "torque";
            actualCalculation = new EngineOutput();
        }

        public void Calculate()
        {
            actualCalculation.torque = (float) characteristicLine.Interpolate(EngineOutput.LastCalculation.rpm);
            if(OnCalculationReady != null)
                OnCalculationReady();
        }

        public void StopCalculation()
        {
            //the function is so small, that it makes no sense
            return; 
        }

        public void StoreResult()
        {
            EngineOutput.LastCalculation.torque = actualCalculation.torque;
            //the rpm will be calculated by the car when its speed is known
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
