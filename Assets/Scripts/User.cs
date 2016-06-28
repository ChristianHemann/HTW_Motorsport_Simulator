using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using CalculationComponents;
using ImportantClasses;


namespace Simulator
{
    public class User
    {
        private User()
        {
            Best_round_times = new SerializableDictionary<string, TimeSpan>();
            Name = "";
            Forname = "";
        }

        [ContainSettings("User")]
        public static User Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new User();
                return _instance;
            }
            set { _instance = value; }
        }
        private static User _instance;
        [Setting("Name")]
        public string Name
        {
            get;
            set;
        }
        [Setting("Forename")]
        public string Forname
        {
            get;
            set;
        }

        public SerializableDictionary<string, TimeSpan> Best_round_times
        { get; set; }

        public void set_Round_time(string name, TimeSpan time)

        {
            TimeSpan best_time;
            if (Best_round_times.TryGetValue(name, out best_time))
            {
                if (best_time > time)
                {

                    Best_round_times[name] = time;
                }

            }
            else
            {
                Best_round_times.Add(name, time);
            }
        }



    }

}
