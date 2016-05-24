using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
    class GearBoxOutput
    {
        private CalculationComponents.GearBox gearbox;
            
        //constructor AeroOutput
        public GearBoxOutput()
        {
        }
        //initialisierung
        private void init()
        {
            gearbox = new GearBox();
            gearbox.Calculate();
        }
        // output 
        private void outp()
        {
            gearbox.StoreResult();
        }
    }
}
