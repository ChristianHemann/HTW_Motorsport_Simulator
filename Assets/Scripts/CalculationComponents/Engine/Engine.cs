using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents
{
    /// <summary>
    /// calculates an engine
    /// </summary>
    public class Engine : ICalculationComponent
    {
        /// <summary>
        /// the carachteeristic line of the engine in torque per revolution speed
        /// </summary>
        [Setting("Characteristic Line (torque/rpm)")]
        public Spline CharacteristicLine { get; set; }

        /// <summary>
        /// the maximum revolution speed of the engine
        /// </summary>
        [Setting("Maximum revolustion speed (rpm)")]
        public int MaxRpm { get; set; }

        /// <summary>
        /// the revolution speed of the engine when no gas is pressed
        /// </summary>
        [Setting("Engine idle (rpm)")]
        public int EngineIdleRpm { get; set; }

        private EngineOutput _actualCalculation; //the result of the actual done calculation

        /// <summary>
        /// calculates an engine. Will be initialized with the standard values
        /// </summary>
        public Engine()
        {
            CharacteristicLine = new Spline(new[] { 0.0, 10000.0 }, new[] { 0.0, 100.0 });
            CharacteristicLine.NameX = "rounds per minute";
            CharacteristicLine.NameY = "torque (Nm)";
            _actualCalculation = new EngineOutput();
            MaxRpm = 10000;
            EngineIdleRpm = 1000;
        }

        /// <summary>
        /// calculate the engineOutputs with the actual inputs
        /// </summary>
        public void Calculate()
        {
            _actualCalculation.Torque = (float)CharacteristicLine.Interpolate(EngineOutput.LastCalculation.Rpm) * InputData.UsedInputData.AccelerationPedal;
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if it necessary to abort it
        /// </summary>
        public void StopCalculation()
        {
            //the function is so small, that it makes no sense
        }

        /// <summary>
        /// stores the result of the calculation to the EngineOutput class
        /// </summary>
        public void StoreResult()
        {
            EngineOutput.LastCalculation.Torque = _actualCalculation.Torque;
            EngineOutput.LastCalculation.Rpm = _actualCalculation.Rpm;
        }

        /// <summary>
        /// invoked after the calculation of the car is ready to calculate the input parameters fof the next calculation step
        /// </summary>
        public void CalculateBackwards()
        {
            if (InputData.UsedInputData.Gear.Equals(0))
                _actualCalculation.Rpm = EngineIdleRpm +
                                         InputData.UsedInputData.AccelerationPedal*(MaxRpm - EngineIdleRpm);
            else
                _actualCalculation.Rpm = GearBoxOutput.LastCalculation.Rpm /
                                         CalculationController.Instance.GearBox.Transmissions[
                                             InputData.UsedInputData.Gear - 1];
            if (_actualCalculation.Rpm < EngineIdleRpm)
                _actualCalculation.Rpm = EngineIdleRpm;
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
