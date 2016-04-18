using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses
{
    /// <summary>
    /// provides a uniform interface for all CalculationComponents
    /// </summary>
    public interface ICalculationComponent
    {
        void Calculate();

        void StopCalculation();

        void StoreResult();

        event CalculationReadyDelegate OnCalculationReady;
    }

    public delegate void CalculationReadyDelegate();
}
