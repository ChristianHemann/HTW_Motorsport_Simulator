
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
        /// The normal Brakemoment per Wheel, when the Brakebalance is 50:50
        /// </summary>
        [Setting("Brakemoment per Wheel (Nm) (Balance 50:50)")]
        public int NormalBrakeMoment { get; set; }

        /// <summary>
        /// the Brakebalance between front and rearAxis. FirstValue is front - SecondValue is rear
        /// </summary>
        [Setting("Brakebalance (0:100 to 100:0)")]
        public MathHelper.Ratio BrakeBalance { get; set; }

        private BrakeOutput _actualCalculation;

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
        /// calculates the brakemoment according to the brakebalance and the actual positon of the brakepedal
        /// </summary>
        public void Calculate()
        {
            _actualCalculation = new BrakeOutput();
            _actualCalculation.BrakeMomentFront = (float) (NormalBrakeMoment*2*InputData.UsedInputData.BrakePedal*
                                                          BrakeBalance.FirstValue);
            _actualCalculation.BrakeMomentRear = (float) (NormalBrakeMoment*2*InputData.UsedInputData.BrakePedal*
                                                         BrakeBalance.SecondValue);
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops executing the Calculate function
        /// </summary>
        public void StopCalculation()
        {
            //Do nothing because the calculation is too short
        }

        /// <summary>
        /// stores the calculation results to the BrakeOutPut class
        /// </summary>
        public void StoreResult()
        {
            BrakeOutput.LastCalculation = _actualCalculation;
        }

        /// <summary>
        /// triggered when the calculation function is ending
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
