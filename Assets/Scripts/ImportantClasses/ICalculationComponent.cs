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
        /// <summary>
        /// used to Calculate the outputs needed for the next calculation component
        /// </summary>
        void Calculate();

        /// <summary>
        /// calles when all calculation are ready to calculate the outputs used in the next frame
        /// </summary>
        void CalculateBackwards();

        /// <summary>
        /// stops the calculation if it is actually running
        /// </summary>
        void StopCalculation();

        /// <summary>
        /// writes the calculation results in the Output Class so that the next component can use them
        /// </summary>
        void StoreResult();

        /// <summary>
        /// called when the actual Calculation Step is ready
        /// </summary>
        event CalculationReadyDelegate OnCalculationReady;
    }

    public delegate void CalculationReadyDelegate();
}
