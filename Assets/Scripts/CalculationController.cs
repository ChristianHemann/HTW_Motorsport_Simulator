using System;
using System.Diagnostics;
using System.Threading;
using System.Xml.Serialization;
using ImportantClasses;
using CalculationComponents;
using ThreadState = System.Threading.ThreadState;

namespace Simulator
{
    /// <summary>
    /// controls the calculation process
    /// </summary>
    public class CalculationController
    {
        /// <summary>
        /// The duration of the actual calculation; updated when calculate is called
        /// </summary>
        public float Duration { get { return _duration;} }
        private float _calculationInterval; //time in seconds between 2 calls of Calculation
        private float _duration; //time in seconds that the actual calculation is taking
        private readonly Thread _workerThread; //The Thread which is calculating the behavior of the car
        private volatile bool _runThread = false; //determines wheather the workerThread shall continue running
        private volatile bool _interruptThread = true; //determines wheather the workerThread shall be interrupted
        private volatile bool _isCalculating = false; //says wheather the workerThread is working
        private volatile EventWaitHandle _calculationEwh; //says the workerThread when to calculate
        private volatile EventWaitHandle _threadInterrupedEwh; //says when the workerThread is securely interrupted
        private volatile EventWaitHandle _threadEndedEwh; //says when the workerThread has ended
        private volatile EventWaitHandle _continueThreadEwh; //says when to continue the interrupted workerThread
        
        /// <summary>
        /// the used Instance of the calculation controller. Please do not set. The setter is just intended for saving
        /// </summary>
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

        /// <summary>
        /// shall performance values be logged?
        /// </summary>
        [Setting("Log performance")]
        public bool LogPerformance = false;

        /// <summary>
        /// calculates the areodynamic of the car
        /// </summary>
        [SettingMenuItem("Aerodynamic")]
        public Aerodynamic Aerodynamic;

        /// <summary>
        /// calculates the brake system of the car
        /// </summary>
        [SettingMenuItem("Brake")]
        public Brake Brake;

        /// <summary>
        /// calculates the engine of the car
        /// </summary>
        [SettingMenuItem("Engine")]
        public Engine Engine;

        /// <summary>
        /// calculates the gearbox of the car
        /// </summary>
        [SettingMenuItem("Gearbox")]
        public GearBox GearBox;

        /// <summary>
        /// calculstes the chassis and the general properties of the car
        /// </summary>
        [SettingMenuItem("Overall car and chassis")]
        public OverallCar OverallCar;

        /// <summary>
        /// calculates the secondary drive of the car
        /// </summary>
        [SettingMenuItem("Secondary Drive")]
        public SecondaryDrive SecondaryDrive;

        /// <summary>
        /// calculates the steering of the car
        /// </summary>
        [SettingMenuItem("Steering")]
        public Steering Steering;

        /// <summary>
        /// calculates the suspension of the car
        /// </summary>
        [SettingMenuItem("Suspension")]
        public Suspension Suspension;

        /// <summary>
        /// calculates the wheels of the car
        /// </summary>
        [SettingMenuItem("Wheels")]
        public Wheels Wheels;

        /// <summary>
        /// controls the calculation process
        /// </summary>
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
            Wheels = new Wheels();

            _workerThread = new Thread(WorkerFunction);
        }

        /// <summary>
        /// Initialize the Instance of the CalculationControler
        /// </summary>
        /// <param name="calculationInterval">the Interval between two calls of the Calculate-function</param>
        public static void Initialize(float calculationInterval)
        {
            Instance._calculationInterval = calculationInterval;
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

        /// <summary>
        /// starts a calculation process if it is not running
        /// </summary>
        public static void Calculate()
        {
            if (!Instance._isCalculating)
            {
                Instance._duration = Instance._calculationInterval;
                lock (InputData.ActualInputData)
                {
                    InputData.UsedInputData = InputData.ActualInputData;
                    //Use the actual InputData in the next calculation step
                }
                Instance._calculationEwh.Set();
            }
            else
            {
                Instance._duration += Instance._calculationInterval;
            }
        }

        /// <summary>
        /// interrupt the calculations after the actual calculation. Continue with Initialize()
        /// </summary>
        public static void Interrupt()
        {
            if (Instance._interruptThread) //if the thread is already interrupted
                return;
            Instance._interruptThread = true;
            Instance._calculationEwh.Set(); //make sure the Thread is not waiting
            Instance._threadInterrupedEwh.WaitOne();
        }

        /// <summary>
        /// terminates the calculation controller. Must be called when quitting the application.
        /// </summary>
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

        /// <summary>
        /// this function handle all the calculations in a new Thread
        /// </summary>
        private void WorkerFunction()
        {
            int i = 0;
            int time;
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
                        //if an exception has occured: log the exception and quit the calculation process
                        Logging.Log(ex.Message+ "\nStackTrace: "+ex.StackTrace, Logging.Classification.CalculationResult,Message.MessageCode.FatalError);
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
            if (exception != null) //rethrow the exception to make debugging easier
                throw exception;
        }

        /// <summary>
        /// this function is used to call all the calculate functions which runs once
        /// </summary>
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

            //call the CalculateBackwards functions
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
            Wheels.CalculateBackwards();
            Wheels.StoreResult();
            OverallCar.CalculateBackwards();
            OverallCar.StoreResult();
            Suspension.CalculateBackwards();
            Suspension.StoreResult();
        }

        /// <summary>
        /// this function is used to call the calculation functions which are calles more than one time
        /// </summary>
        private void DoIterativeWork()
        {
            //iterate as long as the result is not exact enough
            //ToDo: Add loop for iterations
            Track.Instance.Calculate();
            Track.Instance.StoreResult();
            Suspension.Calculate();
            Suspension.StoreResult();
            Wheels.Calculate();
            Wheels.StoreResult();
            OverallCar.Calculate();
            OverallCar.StoreResult();
        }
    }
}
