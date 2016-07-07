using ImportantClasses;

namespace CalculationComponents
{
    /// <summary>
    /// contains all parts of the suspension of the vehicle
    /// </summary>
    public class Suspension : ICalculationComponent
    {
        /// <summary>
        /// the frontaxis of the vehicle
        /// </summary>
        [SettingMenuItem("Front axis")]
        public SuspensionComponents.Axis FrontAxis;
        /// <summary>
        /// the rearaxis of the vehicle
        /// </summary>
        [SettingMenuItem("Rear axis")]
        public SuspensionComponents.Axis RearAxis;

        private byte _calculatedAxis; //save how many wheels are actually Calculated; needed when the wheels can be calculated async

        /// <summary>
        /// contains all parts of the suspension of the vehicle
        /// </summary>
        public Suspension()
        {
            FrontAxis = new SuspensionComponents.Axis(ImportantClasses.Enums.Axis.Front);
            RearAxis = new SuspensionComponents.Axis(ImportantClasses.Enums.Axis.Rear);
            FrontAxis.OnCalculationReady += AxisCalculationReady;
            RearAxis.OnCalculationReady += AxisCalculationReady;
        }

        /// <summary>
        /// calls the calculate function for each part of the suspension
        /// </summary>
        public void Calculate()
        {
            _calculatedAxis = 0;
            FrontAxis.Calculate();
            RearAxis.Calculate();
        }


        /// <summary>
        /// stops the calculation if running
        /// </summary>
        public void StopCalculation()
        {
            FrontAxis.StopCalculation();
            RearAxis.StopCalculation();
        }

        /// <summary>
        /// stores the calculation results of all suspension parts
        /// </summary>
        public void StoreResult()
        {
            FrontAxis.StoreResult();
            RearAxis.StoreResult();
        }

        /// <summary>
        /// calls the calculateBackwards function of all suspension parts
        /// </summary>
        public void CalculateBackwards()
        {
            _calculatedAxis = 0;
            FrontAxis.CalculateBackwards();
            RearAxis.CalculateBackwards();
        }

        /// <summary>
        /// triggered when a wheel has finished its calculation; needed when the wheels can be calculated async
        /// </summary>
        private void AxisCalculationReady()
        {
            if (++_calculatedAxis == 2 && OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
