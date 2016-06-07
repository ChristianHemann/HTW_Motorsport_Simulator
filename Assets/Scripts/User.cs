using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using CalculationComponents;
using ImportantClasses;


namespace Assets.Scripts
{
    class User
    {
        public string Name
        {
            get;
            set;
        }
        public string Forname
        {
            get;
            set;
        }
        public Dictionary<string, TimeSpan> Lastroundtime;
       public Dictionary<string, TimeSpan> Best_round_times
        { get; set; }

        private void Best_round_time()
        {
            TimeSpan span = new TimeSpan();
            Best_round_times.Add("", new TimeSpan(0, 0, 1, 12, 23));

            Best_round_times.TryGetValue("", out span);
         /*   if (span < Best_round_times)
            {
                TimeSpan span = new Lastroundtime   
                    
            }*/

        }
    }

}
