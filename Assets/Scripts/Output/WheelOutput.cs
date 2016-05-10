using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
    class WheelOutput
    {
        private CalculationComponents.Wheel wheel;
        //constructor AeroOutput
        public WheelOutput()
        {
        }
        //initialisierung
        private void init()
        {
            wheel = new Wheel();
            wheel.Calculate();
        }
        // output 
        private void outp()
        {
            wheel.StoreResult();
        }
    }
}
