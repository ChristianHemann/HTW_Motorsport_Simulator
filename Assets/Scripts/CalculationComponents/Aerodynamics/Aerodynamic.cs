using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportantClasses;
using Output;

namespace CalculationComponents
{
    public class Aerodynamic : ICalculationComponent
    {
        private AeroOutput _actualCalculation;

        public Aerodynamic()
        {
            _actualCalculation = new AeroOutput();
        }

        public void Calculate()
        {
            //actually there is no aerodynamic implemented
            _actualCalculation.Downforce = 0;
            _actualCalculation.Drag = 0;
            
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        public void StopCalculation()
        {
            //actually there is no aerodynamic implemented
        }

        public void StoreResult()
        {
            AeroOutput.LastCalculation.Downforce = _actualCalculation.Downforce;
            AeroOutput.LastCalculation.Drag = _actualCalculation.Drag;
        }

        public void CalculateBackwards()
        {
            //here is nothing to calculate
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
