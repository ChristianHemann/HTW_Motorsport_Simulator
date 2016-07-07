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

        public Axis(ImportantClasses.Enums.Axis axis)
        {
            Stabilisator = new Stabilisator();
            LeftWheel = new WheelSuspension((ImportantClasses.Enums.Wheels)(2 * (int)axis));
            RightWheel = new WheelSuspension((ImportantClasses.Enums.Wheels)(2 * (int)axis + 1));
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
