using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportantClasses;

namespace CalculationComponents
{
    public class Track : ICalculationComponent
    {
        [Setting("Distance between cones")]
        public float ConeDistance { get; set; }

        [Setting("Name")]
        public string Name { get; set; }

        public TimeSpan BestRoundTime { get; set; }
        public TimeSpan LastRoundTime { get; set; }

        public void Calculate()
        {
            throw new NotImplementedException();
        }

        public void StopCalculation()
        {
            throw new NotImplementedException();
        }

        public void StoreResult()
        {
            throw new NotImplementedException();
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
