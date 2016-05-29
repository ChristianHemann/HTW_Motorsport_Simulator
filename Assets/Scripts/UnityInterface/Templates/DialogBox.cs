using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace UnityInterface.Templates
{
    public class DialogBox
    {
        private static Rect _windowRect = new Rect((Screen.width - 350f) / 2, (Screen.height - 250f) / 2, 350, 250);
        private static string _windowName, _message, _ok, _cancel;
        private static DialogBoxResult _result = DialogBoxResult.None;

        public static DialogBoxResult Show(string windowName, string message, string ok = "OK", string cancel = "Cancel")
        {
            //Initialize();

            GUI.enabled = true;
            _windowName = windowName;
            _message = message;
            _ok = ok;
            _cancel = cancel;
            _windowRect = GUI.Window(0, _windowRect, DrawWindow, windowName);
            GUI.enabled = false;

            switch (_result)
            {
                case DialogBoxResult.None:
                {
                    return DialogBoxResult.None;
                }
                case DialogBoxResult.Cancel:
                {
                    Reset();
                    return DialogBoxResult.Cancel;
                }
                    case DialogBoxResult.OK:
                {
                    Reset();
                    return DialogBoxResult.OK;
                }
            }
            return DialogBoxResult.None; //should not happen, but otherwise the compiler has a problem
        }

        private static void DrawWindow(int windowID)
        {
            GUI.Label(new Rect(10,10,330,200), _message);
            if(GUI.Button(new Rect(10,205,110,20), _ok))
                _result = DialogBoxResult.OK;
            if(GUI.Button(new Rect(130,205,110,20), _cancel))
                _result = DialogBoxResult.Cancel;
        }

        private static void Reset()
        {
            _result = DialogBoxResult.None;
            _windowRect = new Rect((Screen.width - 350f) / 2, (Screen.height - 250f) / 2, 350, 250);
        }
    }

    public enum DialogBoxResult
    {
        None = 0,
        OK = 1,
        Cancel = 2
    }
}
