namespace WebServer.Interface
{
    public interface IRessourceHandler
    {
        public ResponseContext HandleGet();
        public ResponseContext HandlePost();
        public ResponseContext HandlePut();
        public ResponseContext HandleDelete();
    }
}