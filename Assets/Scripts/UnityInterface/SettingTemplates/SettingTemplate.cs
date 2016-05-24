using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UnityInterface.SettingTemplates
{
    public class SettingTemplate
    {
        public static readonly Dictionary<Type, float> heightDictionary;

        public static void Initialize()
        {
            heightDictionary.Add(typeof(string), 20f);
            heightDictionary.Add(typeof(bool), 20f);

        }

        /*public static T Draw<T>(T value, string name, float height, float width)
        {
            if (heightDictionary.ContainsKey(typeof(T)))
            {
                typeof(SettingTemplate).GetMethods().
                if (typeof(T) == typeof(bool))
                {

                }
            }
            else //if there is no Control defined
            {
                GUI.TextField(new Rect(0, 0, width, height), "For the Type " + typeof(T).Name + " is no Control Defined");
            }

            return null;
        }*/

        public static bool Draw(bool value)
        {
            return false;
        }
    }
}
