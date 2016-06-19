using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace ImportantClasses
{
    /// <summary>
    /// logs data to analyze them later
    /// </summary>
    public class Logging
    {
        /// <summary>
        /// Defines how the message is classified
        /// </summary>
        public enum Classification
        {
            CalculationResult = 0,
            Message,
            SaveLoad
        }

        /// <summary>
        /// The value which should be logged. normally it is a string
        /// </summary>
        public object Value;
        public Classification classification { get; set; }
        public Message.MessageCode Code;

        /// <summary>
        /// contains all logged data since the application was started
        /// </summary>
        public static readonly List<Logging> Data = new List<Logging>();

        /// <summary>
        /// the path to save the logfiles
        /// </summary>
        public static readonly string SavingPath = Path.Combine(Settings.SettingsPath, "Log");
        private static string _currentFile = String.Empty; //the current used LogFile

        /// <summary>
        /// logs data to analyze them later
        /// </summary>
        /// <param name="value">the value to log. normally its a string</param>
        /// <param name="classification">the classification of the log</param>
        /// <param name="code">makes the classification more accurate</param>
        private Logging(object value, Classification classification, Message.MessageCode code)
        {
            this.classification = classification;
            Value = value;
            Code = code;
        }

        /// <summary>
        /// writes a new log
        /// </summary>
        /// <param name="value">the value to log. normally its a string</param>
        /// <param name="classification">the classification of the log</param>
        /// <param name="code">makes the classification more accurate</param>
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
#if UNITY_EDITOR
            if (!Helper.isUnitTest())
                UnityEngine.Debug.Log(log.ToString()); //Show in Console when in Unity Editor
#endif
        }

        /// <summary>
        /// writes the log to the current file
        /// </summary>
        /// <param name="log">the log to save</param>
        private static void Write(Logging log)
        {
            if (!Directory.Exists(SavingPath))
                Directory.CreateDirectory(SavingPath);
            FileStream fileStream = new FileStream(_currentFile,FileMode.Append);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            byte[] text = enc.GetBytes(log.ToString()+"\n");
            fileStream.Write(text, 0, text.Length);
            fileStream.Close();
        }

        /// <summary>
        /// converts the log into a string
        /// </summary>
        /// <returns>the converted log</returns>
        public override string ToString()
        {
            return Value.ToString() + "\n-->classification: " +
                classification.ToString() + "\n-->Code: " + Code.ToString();
        }

        public delegate void NewLogDelegate(Logging log);

        /// <summary>
        /// triggered when a new Log was written
        /// </summary>
        public static event NewLogDelegate OnNewLog;
    }
}
