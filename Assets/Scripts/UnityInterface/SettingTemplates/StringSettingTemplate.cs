using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityInterface.SettingTemplates
{
    class StringSettingTemplate : ISettingTemplate
    {
        private readonly float percentageHeight = 20f;
        private string _value;

        public float PercentageHeight
        {
            get { return percentageHeight; }
        }

        public void Draw(string name, float width, float height)
        {
            throw new NotImplementedException();
        }

        public void Draw(string name, object value, float width, float height)
        {
            throw new NotImplementedException();
        }

        public object GetValue()
        {
            return _value;
        }
    }
}
