using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer.Model
{
    class Message
    {
        public int id;
        public string message;
    }
    interface IMessageController
    {
        public List<Message> GetMessages();
        public Message GetMessage(int id);

        public int Add(string message);
        public bool Delete(int id);
        public bool Update(int id, string message);
    }
}