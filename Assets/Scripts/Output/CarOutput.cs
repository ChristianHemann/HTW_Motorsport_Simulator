using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;
namespace Output
{
    class CarOutput
    {
        private CalculationComponents.OverallCar over;
        //constructor AeroOutput
        public CarOutput()
        {
        }
        //initialisierung
        private void init()
        {
           over = new OverallCar();
            over.Calculate();
        }
        // output 
        private void outp()
        {
            over.StoreResult();
        }
    }
}
