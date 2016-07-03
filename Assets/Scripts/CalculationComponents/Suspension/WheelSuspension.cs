using System;
using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents.SuspensionComponents
{
    public class WheelSuspension : ICalculationComponent
    {
        [Setting("Bellcrank")]
        public BellCrank BellCrank { get; set; }
        [Setting("Damper")]
        public Damper Damper { get; set; }
        [Setting("Pushrod")]
        public PushRod PushRod { get; set; }
        [Setting("Wheelhub")]
        public WheelHub WheelHub { get; set; }
        [Setting("Wishbone")]
        public Wishbone Wishbone { get; set; }

        private readonly Wheels _wheel;
        private readonly SuspensionOutput _actualCalculation;


        public WheelSuspension(Wheels wheel)
        {
            BellCrank = new BellCrank();
            Damper = new Damper();
            PushRod = new PushRod();
            WheelHub = new WheelHub();
            Wishbone = new Wishbone();

            _wheel = wheel;
            _actualCalculation = new SuspensionOutput();
        }

        public void Calculate()
        {
            if ((int)_wheel < 2) //front axis
            {
                _actualCalculation.Torque = 0;
                if ((int)_wheel % 2 == 0) //left wheel
                    _actualCalculation.WheelAngle = SteeringOutput.LastCalculation.WheelAngleLeft;
                else
                    _actualCalculation.WheelAngle = SteeringOutput.LastCalculation.WheelAngleRight;
            }
            else//rearAxis
            {
                _actualCalculation.Torque = SecondaryDriveOutput.LastCalculation.Torque / 2;
                _actualCalculation.WheelAngle = 0;
            }
            _actualCalculation.WheelLoad = CalculationController.Instance.OverallCar.Weight * 9.81f / 4;

            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        public void StoreResult()
        {
            SuspensionOutput.SetLastCalculation(_wheel, _actualCalculation);
        }

        public void CalculateBackwards()
        {
            //actually here is nothing to do
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        public void StopCalculation()
        {
            //actually here the calculation is too short
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
