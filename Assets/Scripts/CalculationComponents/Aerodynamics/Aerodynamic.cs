using ImportantClasses;
using Output;

namespace CalculationComponents
{
    /// <summary>
    /// calculates the aerodynamic of the car
    /// </summary>
    public class Aerodynamic : ICalculationComponent
    {
        private readonly AeroOutput _actualCalculation; //the results of the latest calculation

        /// <summary>
        /// calculates the aerodynamic of the car
        /// </summary>
        public Aerodynamic()
        {
            _actualCalculation = new AeroOutput();
        }

        /// <summary>
        /// calculates the aerodynamic according to the last calculation
        /// </summary>
        public void Calculate()
        {
            //actually there is no aerodynamic implemented
            _actualCalculation.Downforce = 0;
            _actualCalculation.Drag = 0;
            
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if running
        /// </summary>
        public void StopCalculation()
        {
            //actually there is no aerodynamic implemented
        }

        /// <summary>
        /// stores the results of the latest calculation to the AeroOutput class
        /// </summary>
        public void StoreResult()
        {
            AeroOutput.LastCalculation.Downforce = _actualCalculation.Downforce;
            AeroOutput.LastCalculation.Drag = _actualCalculation.Drag;
        }

        /// <summary>
        /// this function is not necessary for the aerodynamic
        /// </summary>
        public void CalculateBackwards()
        {
            //here is nothing to calculate
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
