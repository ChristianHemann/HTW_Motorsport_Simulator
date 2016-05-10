using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;
namespace Output
{
    class EngineOutput
    {
        private CalculationComponents.Engine engine;

        //constructor AeroOutput
        public EngineOutput()
        {
        }
        //initialisierung
        private void init()
        {
            engine = new Engine();
            engine.Calculate();
        }
        // output 
        private void outp()
        {
            engine.StoreResult();
        }
    }
}
