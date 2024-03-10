using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;

namespace MarkRichardson.AdminServices
{

    /// <summary>
    /// A wrapper that calls the methods of the same name in the <see cref="LocalWebService"/>.
    /// </summary>
    public class AdminServicesClient : IDisposable
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="AdminServicesClient"/> class.
        /// </summary>
        public AdminServicesClient()
        {
            Factory = new ChannelFactory<IAdminServices>(new WebHttpBinding(), LocalWebService.BaseAddress);
            Factory.Endpoint.Behaviors.Add(new WebHttpBehavior());
            Channel = Factory.CreateChannel();
        }

        #region Properties ...

        /// <summary>
        /// The channel to the local web service.
        /// </summary>
        public IAdminServices Channel {
            get
            {
                ThrowIfDisposed();
                return channel;
            }
            private set
            {
                channel = value;
            }
        }
        private IAdminServices channel = null;

        /// <summary>
        /// The channel factory that maintains the connection.
        /// </summary>
        private ChannelFactory<IAdminServices> Factory { get; set; }

        #endregion

        /// <summary>
        /// Disposes of all resources used by this object.
        /// </summary>
        #region public void Dispose()
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Disposes of all resources used by this object.
        /// </summary>
        /// <param name="disposing">True to dispose of managed and unmanaged resources.  False to dispose of
        /// unmanaged resources only.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (Factory != null)
                    {
                        Factory.Abort();
                        Factory.Close();
                    }
                }
                Channel = null;
                Factory = null;
                IsDisposed = true;
            }
        }
        /// <summary>
        /// Gets a value indicating whether this object is disposed.
        /// </summary>
        /// <value>True if this object is disposed.  Otherwise, false.</value>
        protected bool IsDisposed { get; private set; }
        /// <summary>
        /// Throws an exception if this object is disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">This object is disposed.</exception>
        protected void ThrowIfDisposed()
        {
            if (IsDisposed) throw new ObjectDisposedException(GetType().FullName);
        }
        /// <summary>
        /// Disposes of unmanaged resources.
        /// </summary>
        ~AdminServicesClient()
        {
            Dispose(false);
        }
        #endregion
    }
}
