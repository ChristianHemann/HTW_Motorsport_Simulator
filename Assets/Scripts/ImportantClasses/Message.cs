using System.Collections.Generic;

namespace ImportantClasses
{
    public class Message
    {
        /// <summary>
        /// classification of a message
        /// </summary>
        public enum MessageCode
        {
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

        public static void Send(string messageText, MessageCode code = MessageCode.Notification)
        {
            Message message = new Message(messageText,code);
            Messages.Add(message);
            if (OnNewMessage != null)
                OnNewMessage(message);
        }
#endregion
#region oop part

        public readonly string MessageText;
        public readonly MessageCode Code;

        private Message(string message, MessageCode code = MessageCode.Notification)
        {
            MessageText = message;
            Code = code;
        }
#endregion
    }
}
