using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Xml.Serialization;
using ImportantClasses;
using CalculationComponents;
using ThreadState = System.Threading.ThreadState;

namespace Simulator
{
    public class CalculationController
    {
        private readonly Thread _workerThread; //The Thread which is calculating the behavior of the car
        private volatile bool _runThread = false; //determines wheather the workerThread shall continue running
        private volatile bool _interruptThread = true; //determines wheather the workerThread shall be interrupted
        private volatile bool _isCalculating = false; //says wheather the workerThread is working
        private volatile EventWaitHandle _calculationEwh; //says the workerThread when to calculate
        private volatile EventWaitHandle _threadInterrupedEwh; //says when the workerThread is securely interrupted
        private volatile EventWaitHandle _threadEndedEwh; //says when the workerThread has ended
        private volatile EventWaitHandle _continueThreadEwh; //says when to continue the interrupted workerThread
        
        [XmlIgnore]
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

        [SettingMenuItem("Overall car and chassis")]
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
            _calculationEwh = new EventWaitHandle(false, EventResetMode.AutoReset);
            _continueThreadEwh = new EventWaitHandle(true, EventResetMode.AutoReset);
            _threadEndedEwh = new EventWaitHandle(false, EventResetMode.AutoReset);
            _threadInterrupedEwh = new EventWaitHandle(false, EventResetMode.AutoReset);

            Aerodynamic = new Aerodynamic();
            Brake = new Brake();
            Engine = new Engine();
            GearBox = new GearBox();
            OverallCar = new OverallCar();
            SecondaryDrive = new SecondaryDrive();
            Steering = new Steering();
            Suspension = new Suspension();
            Wheel = new Wheel();

            _workerThread = new Thread(WorkerFunction);
        }

        public static void Initialize()
        {
            if (Instance._workerThread.ThreadState == ThreadState.WaitSleepJoin) //if the Thread is interrupted
            {
                Instance._interruptThread = false;
                Instance._continueThreadEwh.Set();
            }
            else //if the Thread is not started yet
            {
                Instance._runThread = true;
                Instance._interruptThread = false;
                Instance._workerThread.Start();
            }
        }

        public static void Calculate()
        {
            if (!Instance._isCalculating)
            {
                lock (InputData.ActualInputData)
                {
                    InputData.UsedInputData = InputData.ActualInputData;
                        //Use the actual InputData in the next calculation step
                }
                Instance._calculationEwh.Set();
            }
        }

        public static void Interrupt()
        {
            if (Instance._interruptThread) //if the thread is already interrupted
                return;
            Instance._interruptThread = true;
            Instance._calculationEwh.Set(); //make sure the Thread is not waiting
            Instance._threadInterrupedEwh.WaitOne();
        }

        public static void Terminate()
        {
            if (!Instance._interruptThread)
            {
                Interrupt();
            }
            Instance._runThread = false;
            Instance._continueThreadEwh.Set();
            Instance._threadEndedEwh.WaitOne();
        }

        private void WorkerFunction()
        {
            int i = 0;
            int time = 0;
            Stopwatch performanceWatch = new Stopwatch();
            Exception exception = null;
            while (_runThread)
            {
                _continueThreadEwh.WaitOne(); //wait until the thread is no more interrupted
                while (!_interruptThread)
                {
                    _calculationEwh.WaitOne(); //wait until it is signaled
                    if (_interruptThread || !_runThread) //return without calculating something
                        break;

                    _isCalculating = true;
                    try
                    {
                        //Performance mesurement
                        performanceWatch.Start();
                        i++; //Calculation Number
                        try
                        {
                            DoWork();
                        }
                        catch (NotImplementedException ex)
                        {
                            Logging.Log("Not implemented Exception: " + ex.Message,
                                Logging.Classification.CalculationResult);
                        }
                        //performance mesurement
                        time = performanceWatch.Elapsed.Milliseconds;
                        performanceWatch.Reset();
                        if (LogPerformance)
                            Logging.Log("Iteration: " + i.ToString() + " Time: " + time.ToString() + "ms",
                                Logging.Classification.CalculationResult);
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(ex.Message, Logging.Classification.CalculationResult,Message.MessageCode.FatalError);
                        _interruptThread = true;
                        _runThread = false;
                        _calculationEwh.Set();
                        _continueThreadEwh.Set();
                        exception = ex;
                    }
                    _isCalculating = false;
                }
                _threadInterrupedEwh.Set();
            }
            _threadEndedEwh.Set();
            if (exception != null)
                throw exception;
        }

        private void DoWork()
        {
            Aerodynamic.Calculate();
            Aerodynamic.StoreResult();
            Brake.Calculate();
            Brake.StoreResult();
            Engine.Calculate();
            Engine.StoreResult();
            GearBox.Calculate();
            GearBox.StoreResult();
            SecondaryDrive.Calculate();
            SecondaryDrive.StoreResult();
            Steering.Calculate();
            Steering.StoreResult();

            DoIterativeWork();

            Aerodynamic.CalculateBackwards();
            Aerodynamic.StoreResult();
            Brake.CalculateBackwards();
            Brake.StoreResult();
            Engine.CalculateBackwards();
            Engine.StoreResult();
            GearBox.CalculateBackwards();
            GearBox.StoreResult();
            SecondaryDrive.CalculateBackwards();
            SecondaryDrive.StoreResult();
            Steering.CalculateBackwards();
            Steering.StoreResult();
            Track.Instance.CalculateBackwards();
            Track.Instance.StoreResult();
            Wheel.CalculateBackwards();
            Wheel.StoreResult();
            OverallCar.CalculateBackwards();
            OverallCar.StoreResult();
            Suspension.CalculateBackwards();
            Suspension.StoreResult();
        }

        private void DoIterativeWork()
        {
            //iterate as long as the result is not exact enough
            Track.Instance.Calculate();
            Track.Instance.StoreResult();
            Wheel.Calculate();
            Wheel.StoreResult();
            OverallCar.Calculate();
            OverallCar.StoreResult();
            Suspension.Calculate();
            Suspension.StoreResult();
        }
    }
}
