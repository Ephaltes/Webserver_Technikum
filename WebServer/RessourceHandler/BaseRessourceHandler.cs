using WebServer.API;

namespace WebServer.RessourceHandler
{
    /// <summary>
    /// HandleClass that can handle Get/Post/Put/Delete Requests
    /// </summary>
    public abstract class BaseRessourceHandler
    {
        /// <summary>
        /// Handle Get Request
        /// </summary>
        /// <returns></returns>
        protected abstract ResponseContext HandleGet();
        /// <summary>
        /// Handle Post Request
        /// </summary>
        /// <returns></returns>
        protected abstract ResponseContext HandlePost();
        /// <summary>
        /// Handle Put Request
        /// </summary>
        /// <returns></returns>
        protected abstract ResponseContext HandlePut();
        /// <summary>
        /// Handle Delete Request
        /// </summary>
        /// <returns></returns>
        protected abstract ResponseContext HandleDelete();
        /// <summary>
        /// WrapperFunction that determines wether it is a  Get/Post/Put/Delete Request
        /// </summary>
        /// <returns></returns>
        public abstract ResponseContext Handle();
    }
}