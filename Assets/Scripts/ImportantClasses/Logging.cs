using System;
using System.Collections.Generic;
using System.IO;
using ImportantClasses;

namespace Assets.Scripts.ImportantClasses
{
    public class Logging
    {
        public enum Classification
        {
            CalculationResult = 0,
            Message,
            SaveLoad
        }

        public object Value;
        public Classification classification { get; set; }
        public Message.MessageCode Code;

        /// <summary>
        /// contains all logged data since the application was started
        /// </summary>
        public static readonly List<Logging> Data = new List<Logging>();

        private static readonly string SavingPath = Path.Combine(Settings.SettingsPath, "Log");
        private static string _currentFile = String.Empty;

        private Logging(object value, Classification classification, Message.MessageCode code)
        {
            this.classification = classification;
            Value = value;
            Code = code;
        }

        public static void Log(object value, Classification classification,
            Message.MessageCode code = Message.MessageCode.Notification)
        {
            Logging log = new Logging(value, classification, code);
            Data.Add(log);
            if (string.IsNullOrEmpty(_currentFile))
                _currentFile = Path.Combine(SavingPath, DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss")+".txt");
            Write(log);
            if (OnNewLog != null)
                OnNewLog(log);
        }

        private static void Write(Logging log)
        {
            if (!Directory.Exists(SavingPath))
                Directory.CreateDirectory(SavingPath);
            //if (!File.Exists(_currentFile))
            //    File.Create(_currentFile).Close();
            FileStream fileStream = new FileStream(_currentFile,FileMode.Append);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            byte[] text = enc.GetBytes(log.ToString()+"\n");
            fileStream.Write(text, 0, text.Length);
            fileStream.Close();
        }

        public override string ToString()
        {
            return Value.ToString() + "\n-->classification: " +
                classification.ToString() + "\n-->Code: " + Code.ToString();
        }

        public delegate void NewLogDelegate(Logging log);

        public static event NewLogDelegate OnNewLog;
    }
}
