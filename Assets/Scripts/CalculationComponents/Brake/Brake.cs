using ImportantClasses;
using Output;

namespace CalculationComponents
{
    /// <summary>
    /// calculates the brake
    /// </summary>
    public class Brake : ICalculationComponent
    {
        /// <summary>
        /// The normal Brakemoment per Wheels (Nm), when the Brakebalance is 50:50
        /// </summary>
        [Setting("Brakemoment per Wheels (Nm) (Balance 50:50)")]
        public int NormalBrakeMoment { get; set; }

        /// <summary>
        /// the Brakebalance between front and rearAxis. FirstValue is front - SecondValue is rear
        /// </summary>
        [Setting("Brakebalance front:rear (0:100 to 100:0)")]
        public MathHelper.Ratio BrakeBalance { get; set; }

        private readonly BrakeOutput _actualCalculation;

        /// <summary>
        /// calculates the brake
        /// </summary>
        public Brake()
        {
            _actualCalculation = new BrakeOutput();
            NormalBrakeMoment = 100;
            BrakeBalance = new MathHelper.Ratio(0.5, 0.5);
        }

        /// <summary>
        /// calculates the brakemoment according to the actual positon of the brakepedal
        /// </summary>
        public void Calculate()
        {
            _actualCalculation.BrakeMomentFront = (float) (NormalBrakeMoment*InputData.UsedInputData.BrakePedal*
                                                          BrakeBalance.FirstValue*2);
            _actualCalculation.BrakeMomentRear = (float) (NormalBrakeMoment*InputData.UsedInputData.BrakePedal*
                                                         BrakeBalance.SecondValue*2);
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if running
        /// </summary>
        public void StopCalculation()
        {
            //Do nothing because the calculation is too short
        }

        /// <summary>
        /// stores the calculation results to the BrakeOutput class
        /// </summary>
        public void StoreResult()
        {
            BrakeOutput.LastCalculation.BrakeMomentFront = _actualCalculation.BrakeMomentFront;
            BrakeOutput.LastCalculation.BrakeMomentRear = _actualCalculation.BrakeMomentRear;
        }

        /// <summary>
        /// this function is not necessary for the brake
        /// </summary>
        public void CalculateBackwards()
        {
            //here is nothing to calculate
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
