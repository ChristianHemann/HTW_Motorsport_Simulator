using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityInterface.SettingTemplates
{
    interface ISettingTemplate
    {
        void Draw(string name, object value, float width, float height);
        object GetValue();
        float PercentageHeight { get;}
    }
}
