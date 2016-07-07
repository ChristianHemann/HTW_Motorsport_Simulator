using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents.SuspensionComponents
{
    /// <summary>
    /// the suspension of a single wheel
    /// </summary>
    public class WheelSuspension : ICalculationComponent
    {
        /// <summary>
        /// the Bellcrank of the Wheelsuspension
        /// </summary>
        [Setting("Bellcrank")]
        public BellCrank BellCrank { get; set; }
        /// <summary>
        /// the damper of the WheelSuspension
        /// </summary>
        [Setting("Damper")]
        public Damper Damper { get; set; }
        /// <summary>
        /// The Push or Pullrod of the WheelSuspension
        /// </summary>
        [Setting("Pushrod")]
        public PushRod PushRod { get; set; }
        /// <summary>
        /// the Wheelhub of the WheelSuspension
        /// </summary>
        [Setting("Wheelhub")]
        public WheelHub WheelHub { get; set; }
        /// <summary>
        /// the Wishbone of the WheelSuspension
        /// </summary>
        [Setting("Wishbone")]
        public Wishbone Wishbone { get; set; }

        /// <summary>
        /// for which wheel is the actual WheelSuspension?
        /// </summary>
        private readonly ImportantClasses.Enums.Wheels _wheel;
        private readonly SuspensionOutput _actualCalculation;

        /// <summary>
        /// the suspension of a single wheel
        /// </summary>
        /// <param name="wheel">the wheel for which the suspension is</param>
        public WheelSuspension(ImportantClasses.Enums.Wheels wheel)
        {
            BellCrank = new BellCrank();
            Damper = new Damper();
            PushRod = new PushRod();
            WheelHub = new WheelHub();
            Wishbone = new Wishbone();

            _wheel = wheel;
            _actualCalculation = new SuspensionOutput();
        }

        /// <summary>
        /// Calculates the WheelSuspension
        /// </summary>
        public void Calculate()
        {
            if ((int)_wheel < 2) //front axis
            {
                _actualCalculation.AccelerationTorque = 0;
                if ((int)_wheel % 2 == 0) //left wheel
                    _actualCalculation.WheelAngle = SteeringOutput.LastCalculation.WheelAngleLeft;
                else
                    _actualCalculation.WheelAngle = SteeringOutput.LastCalculation.WheelAngleRight;
            }
            else//rearAxis
            {
                _actualCalculation.AccelerationTorque = SecondaryDriveOutput.LastCalculation.Torque/2;
                _actualCalculation.WheelAngle = 0;
            }
            _actualCalculation.WheelLoad = CalculationController.Instance.OverallCar.Weight * 9.81f / 4;

            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stores the calculation results to the SuspensionOutput class
        /// </summary>
        public void StoreResult()
        {
            SuspensionOutput.SetLastCalculation(_wheel, _actualCalculation);
        }

        /// <summary>
        /// thsi functon is not needed for the WheelSuspension
        /// </summary>
        public void CalculateBackwards()
        {
            //actually here is nothing to do
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if running
        /// </summary>
        public void StopCalculation()
        {
            //actually here the calculation is too short
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
