using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportantClasses;

namespace CalculationComponents
{
    public class OverallCar : ICalculationComponent
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
