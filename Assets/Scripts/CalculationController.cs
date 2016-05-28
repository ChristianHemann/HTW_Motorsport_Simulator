using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MathNet.Numerics.Interpolation;
using System.Threading;
using CalculationComponents;
using ImportantClasses;

namespace Simulator
{
    public class CalculationController
    {
        private readonly Thread workerThread;
        private volatile bool _runThread = true;
        private volatile bool _isCalculating = false;
        private volatile EventWaitHandle _ewh;

        [ContainSettings("Car")]
        public static CalculationController Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new CalculationController();
                return _instance;
            }
            set { return; } //The setter is just to load a value via reflection
        }
        
        private static CalculationController _instance;

        [SettingMenuItem("Engine")]
        public Engine engine;

        [SettingMenuItem("GearBox")]
        public GearBox gearBox;

        [SettingMenuItem("Brake")]
        private Brake brake;

        [SettingMenuItem("Aerodynamic")]
        private Aerodynamic aerodynamics;

        [SettingMenuItem("GearBox")]
        private GearBox gearbox;

        [SettingMenuItem("OverallCar")]
        private OverallCar overallcar;

        [SettingMenuItem("Aerodynamic")]
        private Aerodynamic aerodynamic;

        [SettingMenuItem("SecondaryDrive")]
        private SecondaryDrive secondarydrive;

        [SettingMenuItem("Steering")]
        private Steering steering;

        [SettingMenuItem("Suspension")]
        private Suspension suspension;

        [SettingMenuItem("Track")]
        private Track track;

        [SettingMenuItem("Wheel")]
        private Wheel wheel;

        //just to show the attribute
        [Setting("(just for testing)", 100, 0, 1000, 5)]
        public int torque;

        [Setting("test")]
        public int test { get; set; }

        private CalculationController()
        {
            if (_ewh == null)
                _ewh = new EventWaitHandle(false, EventResetMode.AutoReset);
            workerThread = new Thread(this.WorkerFunction);
            engine = new Engine();
            gearBox = new GearBox();
            torque = 100;
            test = 25;
        }

        public void Initialize()
        {
            _runThread = true;
            workerThread.Start();
        }

        public void Calculate()
        {
            if (!_isCalculating)
                _ewh.Set();
        }

        public void Terminate()
        {
            _runThread = false;
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

                DoWork(); //simulate some work

                //performance mesurement
                time = performanceWatch.Elapsed.Milliseconds;
                performanceWatch.Reset();
                //wirting in the Console of Unity
                UnityEngine.Debug.Log("#Calculation: "+i.ToString()+" Time: "+time.ToString()+"ms");
                _isCalculating = false;
            }
        }

        void DoWork()
        {
            Thread.Sleep(10);
        }
    }
}
