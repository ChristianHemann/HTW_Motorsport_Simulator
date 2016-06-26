using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportantClasses;

namespace CalculationComponents
{
    public class OverallCar : ICalculationComponent
    {
        [Setting("Wheelbase (m)")]
        public float Wheelbase { get; set; }
        [Setting("Weight (kg)")]
        public float Weight { get; set; }
        [Setting("Track width")]
        public float TrackWidth { get; set; }
        [Setting("Drag")]
        public Spline Drag { get; set; }
        
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

        public void CalculateBackwards()
        {
            throw new NotImplementedException();
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
