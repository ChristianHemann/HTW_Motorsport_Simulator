using UnityEngine;

namespace UnityInterface.Templates
{
    public class DialogBox
    {
        private static Rect _windowRect = new Rect((Screen.width - 350f) / 2, (Screen.height - 250f) / 2, 350, 250); //The position and size of the DialogBox
        private static string _message, _ok, _cancel; //the message that is showed and the names of the ok and cancel button
        private static DialogBoxResult _result = DialogBoxResult.None; //the button which was clicked

        /// <summary>
        /// Draws a DialogBox on the screen
        /// </summary>
        /// <param name="windowName">The Title of the DialogBox</param>
        /// <param name="message">The message which will be showed</param>
        /// <param name="ok">The name which will be on the Ok-button</param>
        /// <param name="cancel">The name which will be on the cancel-button</param>
        /// <returns>returns which button was clicked</returns>
        public static DialogBoxResult Show(string windowName, string message, string ok = "Ok", string cancel = "Cancel")
        {
            _message = message;
            _ok = ok;
            _cancel = cancel;
            _windowRect = GUI.Window(0, _windowRect, DrawWindow, windowName);

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
                    case DialogBoxResult.Ok:
                {
                    Reset();
                    return DialogBoxResult.Ok;
                }
            }
            return DialogBoxResult.None; //should not happen, but otherwise the compiler has a problem
        }

        /// <summary>
        /// Draws the buttons and the text of the DialogBox
        /// </summary>
        /// <param name="windowID"></param>
        private static void DrawWindow(int windowID)
        {
            GUI.Label(new Rect(10,10,330,200), _message);
            if(GUI.Button(new Rect(10,205,110,20), _ok))
                _result = DialogBoxResult.Ok;
            if(GUI.Button(new Rect(130,205,110,20), _cancel))
                _result = DialogBoxResult.Cancel;
        }

        /// <summary>
        /// resets the values when the DialogBox is closing
        /// </summary>
        private static void Reset()
        {
            _result = DialogBoxResult.None;
            _windowRect = new Rect((Screen.width - 350f) / 2, (Screen.height - 250f) / 2, 350, 250);
        }
    }

    /// <summary>
    /// says which button was clicked
    /// </summary>
    public enum DialogBoxResult
    {
        None = 0,
        Ok = 1,
        Cancel = 2
    }
}
