using System;
using ImportantClasses;

namespace CalculationComponents.SuspensionComponents
{
    public class Axis : ICalculationComponent
    {
        [Setting("Stabilisator")]
        public Stabilisator Stabilisator { get; set; }
        [SettingMenuItem("Left wheel")]
        public WheelSuspension LeftWheel { get; set; }
        [SettingMenuItem("Right wheel")]
        public WheelSuspension RightWheel { get; set; }

        private CalculationComponents.Axis _axis;

        public Axis(CalculationComponents.Axis axis)
        {
            Stabilisator = new Stabilisator();
            LeftWheel = new WheelSuspension((Wheels)(2 * (int)axis));
            RightWheel = new WheelSuspension((Wheels)(2 * (int)axis + 1));
            _axis = axis;
        }

        public event CalculationReadyDelegate OnCalculationReady;

        public virtual void Calculate()
        {
            LeftWheel.Calculate();
            RightWheel.Calculate();
        }

        public void CalculateBackwards()
        {
            LeftWheel.CalculateBackwards();
            RightWheel.CalculateBackwards();
        }

        public void StopCalculation()
        {
            LeftWheel.StopCalculation();
            RightWheel.StopCalculation();
        }

        public void StoreResult()
        {
            LeftWheel.StoreResult();
            RightWheel.StoreResult();
        }
    }
}
