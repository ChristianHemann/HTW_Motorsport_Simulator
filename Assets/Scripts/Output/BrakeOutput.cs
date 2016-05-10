using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
    class BrakeOutput
    {
        private CalculationComponents.Brake brake;

        //constructor AeroOutput
        public BrakeOutput()
        {
        }
        //initialisierung
        private void init()
        {
            brake = new Brake();
            brake.Calculate();
        }
        // output 
        private void outp()
        {
            brake.StoreResult();
        }
    }
}
