using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serilog.Events;

namespace WebServer.Model
{
    class MessageController : IMessageController
    {
        private readonly List<Message> messageList;
        private int lastid;

        public MessageController()
        {
            messageList = new List<Message>();
        }

        public List<Message> GetMessages()
        {
            return messageList;
        }

        public Message GetMessage(int id)
        {
            return messageList.FirstOrDefault(a => a.id == id);
        }

        public int Add(string message)
        {
            lastid++;
            Message temp = new Message
            {
                message = message,
                id = lastid
            };

            messageList.Add(temp);
            return temp.id;
        }

        public bool Delete(int id)
        {
            Message temp = messageList.FirstOrDefault(a => a.id == id);
            if (temp == null)
                return false;

            messageList.Remove(temp);
            return true;
        }

        public bool Update(int id, string message)
        {
            Message mes = messageList.FirstOrDefault(a => a.id == id);
            if (mes == null)
                return false;
            mes.message = message;
            return true;
        }

    }
}