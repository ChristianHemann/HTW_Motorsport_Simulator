using System;
using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents
{
    /// <summary>
    /// The secondary drive of the vehicle
    /// </summary>
    public class SecondaryDrive : ICalculationComponent
    {
        /// <summary>
        /// the transmission of the secondary drive (rpmIn/rpmOut)
        /// </summary>
        [Setting("Transmission (rpmIn/rpmOut)")]
        public float Transmission { get; set; }

        /// <summary>
        /// the polar area moment of inertia of the secondary drive (m^4)
        /// </summary>
        [Setting("Polar area moment of inertia (m^4)")]
        public float InertiaTorque { get; set; }

        private readonly SecondaryDriveOutput _actualCalculation;

        /// <summary>
        /// The secondary drive of the vehicle
        /// </summary>
        public SecondaryDrive()
        {
            _actualCalculation = new SecondaryDriveOutput();
            Transmission = 1;
            InertiaTorque = 0;
        }

        /// <summary>
        /// Calculates the output torque of the secondary drive
        /// </summary>
        public void Calculate()
        {
            _actualCalculation.Torque = GearBoxOutput.LastCalculation.Torque * Transmission;
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if running
        /// </summary>
        public void StopCalculation()
        {
            //the calculation is too short as it would worth to stop it
        }

        /// <summary>
        /// stores the results of the Calculate-function to the SecondaryDriveOutput class
        /// </summary>
        public void StoreResult()
        {
            SecondaryDriveOutput.LastCalculation.Torque = _actualCalculation.Torque;
            SecondaryDriveOutput.LastCalculation.Rpm = _actualCalculation.Rpm;
        }

        /// <summary>
        /// invoke when the other parts of the car are calculated. Calculates the output rpm
        /// </summary>
        public void CalculateBackwards()
        {
            _actualCalculation.Rpm = OverallCarOutput.LastCalculation.Speed * 60 /
                                     (CalculationController.Instance.Wheels.Diameter * (float)Math.PI);
            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
