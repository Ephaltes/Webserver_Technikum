using System.Collections.Generic;

namespace WebServer.Interface
{
    public class Message
    {
        public int id;
        public string message;
    }
    /// <summary>
    /// Crud interface for a message
    /// </summary>
    public interface IMessage
    {
        public List<Message> GetMessages();
        public Message GetMessage(int id);

        public Message Add(string message);
        public bool Delete(int id);
        public bool Update(int id, string message);
    }
}