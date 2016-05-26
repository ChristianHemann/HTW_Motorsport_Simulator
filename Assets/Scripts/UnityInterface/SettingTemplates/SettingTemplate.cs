using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace UnityInterface.SettingTemplates
{
    public class SettingTemplate
    {
        public static readonly Dictionary<Type, float> heightDictionary = new Dictionary<Type, float>();
        private static SettingTemplate _instance;

        private SettingTemplate() { }

        public static void Initialize()
        {
            _instance = new SettingTemplate();
            heightDictionary.Add(typeof(string), 0.2f);
            heightDictionary.Add(typeof(bool), 0.2f);
        }

        public static object Draw(object oldValue, string name, Rect position)
        {
            //searcch for functions in this class with the same return parameter Type as the values type
            MethodInfo info = typeof(SettingTemplate).GetMethod("Draw",
                new Type[] { oldValue.GetType(), typeof(string), typeof(Rect) });
            if (info != null)
            {
                object[] parameter = new[] {oldValue, name, position};
                return info.Invoke(_instance, parameter);
            }

            //if there is no function defined for that Type
            GUI.TextField(position, "For the Type " + oldValue.GetType().Name + " is no Control Defined");
            return null;
        }

        public bool Draw(bool oldValue, string name, Rect position)
        {
            GUI.Label(new Rect(position.x, position.y, position.width*0.3f, position.height), name);
            return
                GUI.Toggle(
                    new Rect(position.x + position.width*0.3f, position.y, position.width*0.7f, position.height),
                    oldValue, name);
        }

        public string Draw(string oldValue, string name, Rect position)
        {
            GUI.Label(new Rect(position.x, position.y, position.width * 0.3f, position.height), name);
            return GUI.TextField(new Rect(position.x + position.width * 0.3f, position.y, position.width * 0.7f, position.height), oldValue);
        }

        public int Draw(int oldValue, string name, Rect position)
        {
            GUI.Label(new Rect(position.x, position.y, position.width * 0.3f, position.height), name);
            int val;
            string s = GUI.TextField(new Rect(position.x + position.width * 0.3f, position.y, position.width * 0.7f, position.height), oldValue.ToString());
            if (int.TryParse(s, out val))
                return val;
            else
                return oldValue;
        }

        public float Draw(float oldValue, string name, Rect position)
        {
            return (float)Draw((double)oldValue, name, position);
        }

        public double Draw(double oldValue, string name, Rect position)
        {
            GUI.Label(new Rect(position.x, position.y, position.width * 0.3f, position.height), name);
            double val;
            string s = GUI.TextField(new Rect(position.x + position.width * 0.3f, position.y, position.width * 0.7f, position.height), oldValue.ToString());
            if (double.TryParse(s, out val))
                return val;
            else
                return oldValue;
        }
    }
}
