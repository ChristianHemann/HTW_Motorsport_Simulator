﻿using ImportantClasses;
using Output;
using Simulator;

namespace CalculationComponents
{
    /// <summary>
    /// calculates an gearBox. Will be initialized with the standard values
    /// </summary>
    public class GearBox : ICalculationComponent
    {
        //Settings
        /// <summary>
        /// the effficency factor of the gearbox (0 to 1)
        /// </summary>
        [Setting("Efficency (0 to 1)", 0.9, 0.0, 1.0, 3)]
        public float Efficency { get; set; }

        /// <summary>
        /// the number of gears that the gearbox has
        /// </summary>
        [Setting("Number of Gears", 4)]
        public byte Gears {
            get { return _gears; }
            set
            {
                _gears = value;
                float[] buffer = Transmissions;
                Transmissions = new float[value];
                if (buffer != null)
                {
                    int len = buffer.Length < value ? buffer.Length : value;
                    for (int i = 0; i < len; i++)
                    {
                        Transmissions[i] = buffer[i];
                    }
                }
            }
        }
        private byte _gears;

        /// <summary>
        /// the transmission of each gear (rpmIn/rpmOut)
        /// </summary>
        [Setting("Transmission for each gear (rpmIn/rpmOut)")]
        public float[] Transmissions { get; set; }
        
        private readonly GearBoxOutput _actualCalculation; //the result of the actual done calculation

        /// <summary>
        /// calculates an gearBox. Will be initialized with the standard values
        /// </summary>
        public GearBox()
        {
            Gears = 4;
            Efficency = 0.9f;
            _actualCalculation = new GearBoxOutput();
            Transmissions = new float[Gears];
            for (int i = 0; i < Transmissions.Length; i++)
            {
                Transmissions[i] = Transmissions.Length + 1 - i;
            }
        }

        /// <summary>
        /// calculate the GearBoxOutputs with the actual inputs
        /// </summary>
        public void Calculate()
        {
            if (InputData.UsedInputData.Gear == 0)
                _actualCalculation.Torque = 0;
            else
                _actualCalculation.Torque = EngineOutput.LastCalculation.Torque*Efficency*Transmissions[InputData.UsedInputData.Gear-1];

            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if running
        /// </summary>
        public void StopCalculation()
        {
            //the function is so small, that it makes no sense
        }

        /// <summary>
        /// stores the result of the calculation to the GearBoxOutput class
        /// </summary>
        public void StoreResult()
        {
            GearBoxOutput.LastCalculation.Torque = _actualCalculation.Torque;
            GearBoxOutput.LastCalculation.Rpm = _actualCalculation.Rpm;
        }

        /// <summary>
        /// called when all Calculation Steps are ready to calculate the gearBox rpm
        /// </summary>
        public void CalculateBackwards()
        {
            _actualCalculation.Rpm = SecondaryDriveOutput.LastCalculation.Rpm*
                                     CalculationController.Instance.SecondaryDrive.Transmission;

            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
