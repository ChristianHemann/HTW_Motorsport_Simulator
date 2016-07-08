using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

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
            None = 0,
            CalculationResult,
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
        /// the path to save the logfiles
        /// </summary>
        public static readonly string SavingPath = Path.Combine(Settings.SettingsPath, "Log");

        private static readonly string _logContentFile = Path.Combine(SavingPath, "LogContent.txt");
        private static readonly string _logFile = Path.Combine(SavingPath, "Log.xml"); //the current used LogFile
        private static System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();

        private static string _logFileContent
        {
            get
            {
                StringBuilder builder = new StringBuilder("");
                builder.AppendLine("<?xml version=\"1.0\"?>");
                builder.AppendLine("<!DOCTYPE logfile[");
                builder.AppendLine("<!ENTITY logs");
                builder.AppendFormat("SYSTEM \"{0}\" >", _logContentFile);
                builder.AppendLine("]>");
                builder.AppendLine("<logfile>");
                builder.AppendLine("&logs;");
                builder.AppendLine("</logfile>");
                return builder.ToString();
            }
        }

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
            Write(log);
            if (OnNewLog != null)
                OnNewLog(log);
#if UNITY_EDITOR
            if (!Helper.IsUnitTest())
                UnityEngine.Debug.Log(log.ToString()); //Show in Console when in Unity Editor
#endif
        }

        /// <summary>
        /// gets all the logs which fits the parameters
        /// </summary>
        /// <param name="classification">return just logs with this classification; none = no filter</param>
        /// <param name="code">return just logs with this messageCode; none = no filter</param>
        /// <param name="from">The date of the logs must be from this date or later; works just in combination with interval</param>
        /// <param name="interval">the time interval to get the logs; just works in combination with from</param>
        /// <param name="count">the maximum number of logs to return</param>
        /// <param name="startAt">ignore the first 'startAt' logs</param>
        /// <returns>all logs which fits the set parameters</returns>
        public static Logging[] GetLogs(Classification classification = Classification.None,
            Message.MessageCode code = Message.MessageCode.None,
            DateTime from = default(DateTime), TimeSpan interval = default(TimeSpan), uint count = 20, uint startAt = 0)
        {
            List<Logging> logs = new List<Logging>();

            XmlTextReader tr = new XmlTextReader(_logFile);
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.ProhibitDtd = false; //dtd is used
            readerSettings.ValidationType = ValidationType.None;
            XmlReader reader = XmlReader.Create(tr, readerSettings);

            XmlDocument doc = new XmlDocument();
            doc.Load(reader);
            XmlNodeList list = doc.SelectNodes("//Logging");
            if (list == null)
                return logs.ToArray();
            uint i = 0;
            if (from != default(DateTime) && interval != default(TimeSpan))
            {
                foreach (XmlElement element in list)
                {
                    if (i == startAt + count)
                        break;
                    if ((classification == Classification.None ||
                         element.ChildNodes[1].InnerText == classification.ToString()) &&
                        (code == Message.MessageCode.None || element.ChildNodes[2].InnerText == code.ToString()))
                    {
                        DateTime dateTime = Convert.ToDateTime(element.ChildNodes[3].InnerText,
                            DateTimeFormatInfo.InvariantInfo);
                        if ((dateTime >= from && dateTime <= from + interval)
                            && (i++ >= startAt))
                        {
                            logs.Add(new Logging(element.ChildNodes[0].InnerText,
                                (Classification)Enum.Parse(typeof(Classification), element.ChildNodes[1].InnerText),
                                (Message.MessageCode)Enum.Parse(typeof(Message.MessageCode), element.ChildNodes[2].InnerText)));
                        }
                    }
                }
            }
            else
            {
                foreach (XmlElement element in list)
                {
                    if (i == startAt + count)
                        break;
                    if ((classification == Classification.None ||
                         element.ChildNodes[1].InnerText == classification.ToString()) &&
                        (code == Message.MessageCode.None || element.ChildNodes[2].InnerText == code.ToString()) &&
                        i++ >= startAt)
                    {
                        logs.Add(new Logging(element.ChildNodes[0].InnerText,
                            (Classification)Enum.Parse(typeof(Classification), element.ChildNodes[1].InnerText),
                            (Message.MessageCode)Enum.Parse(typeof(Message.MessageCode), element.ChildNodes[2].InnerText)));
                    }
                }
            }
            return logs.ToArray();
        }

        /// <summary>
        /// writes the log to the logfile
        /// </summary>
        /// <param name="log">the log to save</param>
        private static void Write(Logging log)
        {
            //TODO: Write the logs async to improve the performance
            lock (_logFile)
            {
                lock (_logContentFile)
                {
                    if (!Directory.Exists(SavingPath))
                        Directory.CreateDirectory(SavingPath);
                    if (!File.Exists(_logFile))
                    {
                        FileStream stream = new FileStream(_logFile, FileMode.Create);
                        byte[] content = enc.GetBytes(_logFileContent);
                        stream.Write(content, 0, content.Length);
                        stream.Close();
                    }
                    if (!File.Exists(_logContentFile))
                        File.Create(_logContentFile).Close();
                    StreamWriter sw = File.AppendText(_logContentFile);
                    XmlTextWriter xtw = new XmlTextWriter(sw);

                    xtw.WriteStartElement("Logging");
                    xtw.WriteElementString("Value", log.Value.ToString());
                    xtw.WriteElementString("classification", log.classification.ToString());
                    xtw.WriteElementString("Code", log.Code.ToString());
                    xtw.WriteElementString("date", DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo));
                    xtw.WriteEndElement();

                    xtw.Close();
                    sw.Close();
                }
            }
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
