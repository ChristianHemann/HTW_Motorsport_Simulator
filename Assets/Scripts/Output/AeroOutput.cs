﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
   public class AeroOutput
    {
        //private CalculationComponents.Aerodynamic aerodynamic;

        ////constructor AeroOutput
        //public AeroOutput ()
        //{
        //}
        ////initialisierung
        //private void init()
        //{
        //    aerodynamic = new Aerodynamic();
        //    aerodynamic.Calculate();
        //}
        //// output 
        //private void outp()
        //{
        //    aerodynamic.StoreResult();
        //}
        public float downforce { get; set; }
        public float drag { get; set; }

    }

}
