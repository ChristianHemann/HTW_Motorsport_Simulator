using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ImportantClasses
{
    public static class Xml
    {
        public static void WriteXml(string path, object obj)
        {
            FileStream stream = new FileStream(path,FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            serializer.Serialize(stream, obj);
            stream.Close();
        }

        public static T ReadXml<T>(string path)
        {
            if(!File.Exists(path))
                throw new FileNotFoundException("Could not found the specified file: "+path);
            FileStream stream = new FileStream(path, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            object obj = serializer.Deserialize(stream);
            stream.Close();
            return (T)obj;
        }

        public static object ReadXml(string path, Type type)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Could not found the specified file: " + path);
            FileStream stream = new FileStream(path, FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(type);
            object obj = serializer.Deserialize(stream);
            stream.Close();
            return obj;
        }
    }
}
