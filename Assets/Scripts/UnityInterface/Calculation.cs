using UnityEngine;
using ImportantClasses;
using Simulator;

namespace UnityInterface
{
    public class Calculation : MonoBehaviour
    {

        // Initialization
        private void Start()
        {
            CalculationController.Initialize();
            InputData.ActualInputData = new InputData(0, 0, 0, 0);
        }

        //called periodically
        private void FixedUpdate()
        {
            // Get Input Data
            lock (InputData.ActualInputData)
            {
                InputData.ActualInputData = new InputData(Input.GetAxis("AccelerationPedal"),
                    Input.GetAxis("BrakePedal"), Input.GetAxis("Steering"), InputData.ActualInputData.Gear);
            }
            if(!RaceMenu.ShowMenu)
                CalculationController.Calculate();
        }

        //called once per frame
        private void Update()
        {
            //the gear must be read in the Update function
            lock (InputData.ActualInputData)
            {
                if (Input.GetButtonDown("ShiftUp") &&
                    InputData.ActualInputData.Gear < CalculationController.Instance.GearBox.Gears)
                    InputData.ActualInputData.Gear++;
                if (Input.GetButtonDown("ShiftDown") && InputData.ActualInputData.Gear > 0)
                    InputData.ActualInputData.Gear--;
            }
        }

        //quitting
        private void OnApplicationQuit()
        {
            CalculationController.Terminate();
        }

        private void OnDestroy()
        {
            CalculationController.Interrupt();
        }
    }
}
