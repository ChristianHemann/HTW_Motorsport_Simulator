using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ImportantClasses
{
    /// <summary>
    /// provides functions to save and load objects as xml-files 
    /// </summary>
    public static class Xml
    {
        /// <summary>
        /// saves an object to a xml file
        /// </summary>
        /// <param name="path">the path to the file on the filesystem</param>
        /// <param name="obj">the object that shall be saved</param>
        public static void WriteXml(string path, object obj)
        {
            FileStream stream = new FileStream(path,FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(stream, obj);
            stream.Close();
            if (FinishedWriting != null)
                FinishedWriting(obj.GetType());
        }

        /// <summary>
        /// creates an object of the given Type from file
        /// </summary>
        /// <typeparam name="T">The objects Type</typeparam>
        /// <param name="path">the path to the file on the filesystem</param>
        /// <returns>the object which was read</returns>
        public static T ReadXml<T>(string path)
        {
            if(!File.Exists(path))
                throw new FileNotFoundException("Could not found the specified file: "+path);
            FileStream stream = new FileStream(path, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            object obj = serializer.Deserialize(stream);
            stream.Close();
            if (FinishedReading != null)
                FinishedReading(typeof(T));
            return (T)obj;
        }

        /// <summary>
        /// creates an object of the given Type from file
        /// </summary>
        /// <param name="path">the path to the file on the filesystem</param>
        /// <param name="type">The objects Type</param>
        /// <returns>the object which was read</returns>
        public static object ReadXml(string path, Type type)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Could not found the specified file: " + path);
            FileStream stream = new FileStream(path, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(type);
            object obj = serializer.Deserialize(stream);
            stream.Close();
            if (FinishedReading != null)
                FinishedReading(type);
            return obj;
        }

        public delegate void OnReadWriteFinished(Type objectType);

        /// <summary>
        /// invoked when a file is read
        /// </summary>
        public static event OnReadWriteFinished FinishedReading;

        /// <summary>
        /// invoked when a file is written
        /// </summary>
        public static event OnReadWriteFinished FinishedWriting;
    }
}
