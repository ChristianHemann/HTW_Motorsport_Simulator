using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
    class SecondaryDriveOutput
    {
        private CalculationComponents.SecondaryDrive driveoutput;
        //constructor AeroOutput
        public SecondaryDriveOutput()
        {
        }
        //initialisierung
        private void init()
        {
            driveoutput = new SecondaryDrive();
            driveoutput.Calculate();
        }
        // output 
        private void outp()
        {
            driveoutput.StoreResult();
        }
    }
}
