using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
    class TrackOutput
    {
        private CalculationComponents.Track track;
        //constructor AeroOutput
        public TrackOutput()
        {
        }
        //initialisierung
        private void init()
        {
            track = new Track();
            track.Calculate();
        }
        // output 
        private void outp()
        {
            track.StoreResult();
        }
    }
}
