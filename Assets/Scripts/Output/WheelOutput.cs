using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
   public class WheelOutput
    {
        //private CalculationComponents.Wheel wheel;
        ////constructor AeroOutput
        //public WheelOutput()
        //{
        //}
        ////initialisierung
        //private void init()
        //{
        //    wheel = new Wheel();
        //    wheel.Calculate();
        //}
        //// output 
        //private void outp()
        //{
        //    wheel.StoreResult();
        //}
        public float force { get; set; }
        public int slip { get; set; }
    }
}
