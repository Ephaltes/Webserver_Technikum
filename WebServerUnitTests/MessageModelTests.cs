using NUnit.Framework;
using WebServer.Model;

namespace WebServerUnitTests
{
    public class MessageModelTests
    {
        [Test]
        public void CreateMessage()
        {
            MessageModel model = new MessageModel();
            model.Add("Meine Erste Nachricht");
            
            Assert.That(model.GetMessage(1).message == "Meine Erste Nachricht");
        }
        
        [Test]
        public void ReadMessage()
        {
            MessageModel model = new MessageModel();
            model.Add("Meine Erste Nachricht");
            model.Add("Meine Zweite Nachricht");
            model.Add("Meine Dritte Nachricht");

            var msg = model.GetMessage(2);
            
            Assert.That(msg.id==2 && msg.message=="Meine Zweite Nachricht");
        }
        
        [Test]
        public void GetMessages()
        {
            MessageModel model = new MessageModel();
            model.Add("Meine Erste Nachricht");
            model.Add("Meine Zweite Nachricht");
            model.Add("Meine Dritte Nachricht");

            var msg = model.GetMessages();
            
            Assert.That(msg.Count == 3);
        }
        
        [Test]
        public void UpdateMessage()
        {
            MessageModel model = new MessageModel();
            model.Add("Meine Erste Nachricht");
            model.Add("Meine Zweite Nachricht");
            model.Add("Meine Dritte Nachricht");

            var msg = model.Update(2,"Meine Fünfte Nachricht");
            
            Assert.That(msg);
        }
        
        [Test]
        public void UpdateMessageFailed()
        {
            MessageModel model = new MessageModel();
            model.Add("Meine Erste Nachricht");
            model.Add("Meine Zweite Nachricht");
            model.Add("Meine Dritte Nachricht");

            var msg = model.Update(5,"Meine Fünfte Nachricht");
            
            Assert.That(!msg);
        }
        
        [Test]
        public void DeleteMessage()
        {
            MessageModel model = new MessageModel();
            model.Add("Meine Erste Nachricht");
            model.Add("Meine Zweite Nachricht");
            model.Add("Meine Dritte Nachricht");

            var msg = model.Delete(2);
            
            Assert.That(msg);
        }
        
        [Test]
        public void DeleteMessageFailed()
        {
            MessageModel model = new MessageModel();
            model.Add("Meine Erste Nachricht");
            model.Add("Meine Zweite Nachricht");
            model.Add("Meine Dritte Nachricht");

            var msg = model.Delete(5);
            
            Assert.That(!msg);
        }
        
    }
}