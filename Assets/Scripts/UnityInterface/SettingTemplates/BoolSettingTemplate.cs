using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityInterface.SettingTemplates
{
    class BoolSettingTemplate : ISettingTemplate
    {
        private readonly float percentageHeight = 20f;
        private bool _value;

        public void Draw(string name, object value, float width, float height)
        {
            //_value = GUI.Toggle(new Rect(0, 0, width, height), value, name);
        }

        public object GetValue()
        {
            return _value;
        }

        public float PercentageHeight
        {
            get { return percentageHeight; }
        }
    }
}
