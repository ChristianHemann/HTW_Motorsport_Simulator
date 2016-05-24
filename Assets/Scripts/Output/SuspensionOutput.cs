using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;
namespace Output
{
    public class SuspensionOutput
    {
        //private CalculationComponents.Suspension suspension;
        ////constructor AeroOutput
        //public SuspensionOutput()
        //{
        //}
        ////initialisierung
        //private void init()
        //{
        //    suspension = new Suspension();
        //    suspension.Calculate();
        //}
        //// output 
        //private void outp()
        //{
        //    suspension.StoreResult();
        //}
        public float torq { get; set; }
        public float wheelload { get; set; }
        public float wheelangle {get; set;}
        public float camb { get; set; }
        public float incl { get; set; }
        public float track { get; set; }
        public float coastdo { get; set; }
    }
}
