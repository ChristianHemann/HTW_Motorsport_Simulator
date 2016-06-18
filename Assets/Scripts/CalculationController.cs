using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using ImportantClasses;
using CalculationComponents;

namespace Simulator
{
    public class CalculationController
    {
        private readonly Thread _workerThread; //The Thread which is calculating the behavior of the car
        private volatile bool _runThread = false; //determines wheather the workerThread shall continue running
        private volatile bool _isCalculating = false; //says wheather the workerThread is working
        private volatile EventWaitHandle _ewh; //says the workerThread when to calculate
        
        public InputData InputDataBuffer { get; set; } 

        [ContainSettings("Car")]
        public static CalculationController Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new CalculationController();
                return _instance;
            }
            set { _instance = value; } //The setter is just to load a value via reflection
        }
        
        private static CalculationController _instance;

        [Setting("Log performance")]
        public bool LogPerformance = false;

        [SettingMenuItem("Aerodynamic")]
        public Aerodynamic Aerodynamic;

        [SettingMenuItem("Brake")]
        public Brake Brake;

        [SettingMenuItem("Engine")]
        public Engine Engine;

        [SettingMenuItem("Gearbox")]
        public GearBox GearBox;

        [SettingMenuItem("Chassis")]
        public OverallCar OverallCar;

        [SettingMenuItem("Secondary Drive")]
        public SecondaryDrive SecondaryDrive;

        [SettingMenuItem("Steering")]
        public Steering Steering;

        [SettingMenuItem("Suspension")]
        public Suspension Suspension;

        [SettingMenuItem("Wheel")]
        public Wheel Wheel;

        private CalculationController()
        {
            _ewh = new EventWaitHandle(false, EventResetMode.AutoReset);
            _workerThread = new Thread(WorkerFunction);
            Aerodynamic = new Aerodynamic();
            Brake = new Brake();
            Engine = new Engine();
            GearBox = new GearBox();
            OverallCar = new OverallCar();
            SecondaryDrive = new SecondaryDrive();
            Steering = new Steering();
            Suspension = new Suspension();
            Wheel = new Wheel();
        }

        public static void Initialize()
        {
            Instance._runThread = true;
            Instance._workerThread.Start();
        }

        public static void Calculate()
        {
            if (!Instance._isCalculating)
            {
                InputData.UsedInputData = InputData.ActualInputData; //Use the actual InputData in the next calculation step
                Instance._ewh.Set();
            }
        }

        public static void Terminate()
        {
            Instance._runThread = false;
        }

        private void WorkerFunction()
        {
            int i = 0;
            int time = 0;
            Stopwatch performanceWatch = new Stopwatch();
            while (_runThread)
            {
                _ewh.WaitOne(); //wait until it is signaled
                _isCalculating = true;

                //Performance mesurement
                performanceWatch.Start();
                i++; //Calculation Number
                try
                {
                    Aerodynamic.Calculate();
                    Brake.Calculate();
                    Engine.Calculate();
                    GearBox.Calculate();
                    SecondaryDrive.Calculate();
                    Steering.Calculate();

                    DoIterativeWork();
                }
                catch (NotImplementedException ex)
                {
                    Logging.Log("Not implemented Exception: "+ex.Message,Logging.Classification.CalculationResult);
                }
                //performance mesurement
                time = performanceWatch.Elapsed.Milliseconds;
                performanceWatch.Reset();
                if(LogPerformance)
                    Logging.Log("Iteration: "+i.ToString()+" Time: "+time.ToString()+"ms",Logging.Classification.CalculationResult);
                _isCalculating = false;
            }
        }

        private void DoIterativeWork()
        {
            //iterate as long as the result is not exact enough
            Track.Instance.Calculate();
            Wheel.Calculate();
            OverallCar.Calculate();
            Suspension.Calculate();
        }
    }
}
