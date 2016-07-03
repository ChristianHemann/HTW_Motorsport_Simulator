using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents.SuspensionComponents;
using ImportantClasses;

namespace CalculationComponents
{
    public class Suspension : ICalculationComponent
    {
        [SettingMenuItem("Front axis")]
        public SuspensionComponents.Axis FrontAxis;
        [SettingMenuItem("Rear axis")]
        public SuspensionComponents.Axis RearAxis;

        private byte _calculatedWheels; //save how many wheels are actually Calculated

        public Suspension()
        {
            FrontAxis = new SuspensionComponents.Axis(Axis.Front);
            RearAxis = new SuspensionComponents.Axis(Axis.Rear);
            FrontAxis.RightWheel.OnCalculationReady += WheelCalculationReady;
            FrontAxis.LeftWheel.OnCalculationReady += WheelCalculationReady;
            RearAxis.RightWheel.OnCalculationReady += WheelCalculationReady;
            RearAxis.LeftWheel.OnCalculationReady += WheelCalculationReady;
        }

        public void Calculate()
        {
            _calculatedWheels = 0;
            FrontAxis.Calculate();
            RearAxis.Calculate();
        }

        public void StopCalculation()
        {
            FrontAxis.StopCalculation();
            RearAxis.StopCalculation();
        }

        public void StoreResult()
        {
            FrontAxis.StoreResult();
            RearAxis.StoreResult();
        }

        public void CalculateBackwards()
        {
            _calculatedWheels = 0;
            FrontAxis.CalculateBackwards();
            RearAxis.CalculateBackwards();
        }

        private void WheelCalculationReady()
        {
            if (++_calculatedWheels == 4 && OnCalculationReady != null)
                OnCalculationReady();
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
