using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Serilog.Events;
using WebServer.Interface;

namespace WebServer.Model
{
    /// <summary>
    /// In Memory Messages
    /// </summary>
    public class MessageModel : IMessage
    {
        /// <summary>
        /// List of Messages send to server
        /// </summary>
        private readonly List<Message> messageList;
        /// <summary>
        /// Lockobject for Concurrent uses
        /// </summary>
        private object lockobject = new object();
        /// <summary>
        /// Last id used for a message
        /// </summary>
        private int lastid;

        public MessageModel()
        {
            messageList = new List<Message>();
        }

        /// <summary>
        /// Returns all messages
        /// </summary>
        /// <returns></returns>
        public List<Message> GetMessages()
        {
            return messageList;
        }

        /// <summary>
        /// Returns a single message
        /// </summary>
        /// <param name="id">messageID to return</param>
        /// <returns></returns>
        public Message GetMessage(int id)
        {
            return messageList.FirstOrDefault(a => a.id == id);
        }

        /// <summary>
        /// Adds a message to the list
        /// </summary>
        /// <param name="message">Message to be added</param>
        /// <returns></returns>
        public Message Add(string message)
        {
            lock (lockobject)
            {
                lastid++;
                Message temp = new Message
                {
                    message = message,
                    id = lastid
                };
                messageList.Add(temp);
                return temp; 
            }
        }

        /// <summary>
        /// Deletes a given message by id
        /// </summary>
        /// <param name="id">MessageID to be deleted</param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            lock (lockobject)
            {
                Message temp = messageList.FirstOrDefault(a => a.id == id);
                if (temp == null)
                    return false;

                messageList.Remove(temp);
                return true;
            }
        }

        /// <summary>
        /// Replacing whole message with a new message
        /// </summary>
        /// <param name="id">MessageID to be replaced</param>
        /// <param name="message">new Message</param>
        /// <returns></returns>
        public bool Update(int id, string message)
        {
            lock (lockobject)
            {
                Message mes = messageList.FirstOrDefault(a => a.id == id);
                if (mes == null)
                    return false;
                mes.message = message;
                return true;
            }
        }
    }
}
