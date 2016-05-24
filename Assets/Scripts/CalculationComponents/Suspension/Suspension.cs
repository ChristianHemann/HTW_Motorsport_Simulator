using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents.SuspensionComponents;
using ImportantClasses;

namespace CalculationComponents
{
    public class Suspension : ICalculationComponent
    {
        public void Calculate()
        {
            throw new NotImplementedException();
        }

        public void StopCalculation()
        {
            throw new NotImplementedException();
        }

        public void StoreResult()
        {
            throw new NotImplementedException();
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
