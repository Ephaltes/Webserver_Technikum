namespace WebServer.Interface
{
    public abstract class BaseRessourceHandler
    {
        protected abstract ResponseContext HandleGet();
        protected abstract ResponseContext HandlePost();
        protected abstract ResponseContext HandlePut();
        protected abstract ResponseContext HandleDelete();
        public abstract ResponseContext Handle();
    }
}