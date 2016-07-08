using System.Collections.Generic;

namespace ImportantClasses
{
    /// <summary>
    /// Sends messages to be shown in the GUI
    /// </summary>
    public class Message
    {
        /// <summary>
        /// classification of a message
        /// </summary>
        public enum MessageCode
        {
            None = 0,
            Notification,
            Warning,
            Error,
            FatalError
        }

#region static part
        private static List<Message> _messages;
        public delegate void NewMessageDelegate(Message message);
        public static event NewMessageDelegate OnNewMessage;

        /// <summary>
        /// contains all Messages since the application was started
        /// </summary>
        public static List<Message> Messages
        {
            get
            {
                if (_messages == null)
                    _messages = new List<Message>();
                return _messages;
            }
        }

        /// <summary>
        /// sends a message to show it
        /// </summary>
        /// <param name="messageText"></param>
        /// <param name="code"></param>
        public static void Send(string messageText, MessageCode code = MessageCode.Notification)
        {
            Message message = new Message(messageText,code);
            Messages.Add(message);
            if (OnNewMessage != null)
                OnNewMessage(message);
        }
#endregion
#region oop part

        /// <summary>
        /// the content of the message
        /// </summary>
        public readonly string MessageText;
        /// <summary>
        /// how the message is classificated
        /// </summary>
        public readonly MessageCode Code;


        private Message(string message, MessageCode code = MessageCode.Notification)
        {
            MessageText = message;
            Code = code;
        }
#endregion
    }
}
