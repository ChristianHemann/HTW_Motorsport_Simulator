using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;
namespace Output
{
    class SuspensionOutput
    {
        private CalculationComponents.Suspension suspension;
        //constructor AeroOutput
        public SuspensionOutput()
        {
        }
        //initialisierung
        private void init()
        {
            suspension = new Suspension();
            suspension.Calculate();
        }
        // output 
        private void outp()
        {
            suspension.StoreResult();
        }
    }
}
