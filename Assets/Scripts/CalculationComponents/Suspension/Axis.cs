using ImportantClasses;

namespace CalculationComponents.SuspensionComponents
{
    /// <summary>
    /// represents an axis of the vehicle
    /// </summary>
    public class Axis : ICalculationComponent
    {
        /// <summary>
        /// the stabilisator of the axis
        /// </summary>
        [SettingMenuItem("Stabilisator")]
        public Stabilisator Stabilisator { get; set; }
        /// <summary>
        /// The left wheel of the axis
        /// </summary>
        [SettingMenuItem("Left wheel")]
        public WheelSuspension LeftWheel { get; set; }
        /// <summary>
        /// the right wheel of the axis
        /// </summary>
        [SettingMenuItem("Right wheel")]
        public WheelSuspension RightWheel { get; set; }

        private int _calculatedWheels;

        /// <summary>
        /// represents an axis of the vehicle
        /// </summary>
        /// <param name="axis">Front- or rearaxis</param>
        public Axis(ImportantClasses.Enums.Axis axis)
        {
            Stabilisator = new Stabilisator();
            LeftWheel = new WheelSuspension((ImportantClasses.Enums.Wheels)(2 * (int)axis));
            RightWheel = new WheelSuspension((ImportantClasses.Enums.Wheels)(2 * (int)axis + 1));
            LeftWheel.OnCalculationReady += WheelCalculationReady;
            RightWheel.OnCalculationReady += WheelCalculationReady;
        }

        /// <summary>
        /// this constructor is just to provide xml-functions
        /// </summary>
        private Axis() { }

        /// <summary>
        /// calls the Calculate function of each Wheel of the axis
        /// </summary>
        public virtual void Calculate()
        {
            _calculatedWheels = 0;
            LeftWheel.Calculate();
            RightWheel.Calculate();
        }

        /// <summary>
        /// calls the calculateBackwards function of each wheel of the axis
        /// </summary>
        public void CalculateBackwards()
        {
            _calculatedWheels = 0;
            LeftWheel.CalculateBackwards();
            RightWheel.CalculateBackwards();
        }

        /// <summary>
        /// stops the calculation if running
        /// </summary>
        public void StopCalculation()
        {
            LeftWheel.StopCalculation();
            RightWheel.StopCalculation();
        }

        /// <summary>
        /// stores the results of each axis
        /// </summary>
        public void StoreResult()
        {
            LeftWheel.StoreResult();
            RightWheel.StoreResult();
        }

        /// <summary>
        /// invoked when a wheel has finished its calculation
        /// </summary>
        private void WheelCalculationReady()
        {
            if (_calculatedWheels == 2 && OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
