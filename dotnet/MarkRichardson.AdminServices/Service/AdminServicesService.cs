using System.Diagnostics;
using System.ServiceModel;

namespace MarkRichardson.AdminServices
{

    /// <summary>
    /// The implementation of the available administration operations.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class AdminServicesService : IAdminServices
    {

        /// <summary>
        /// Initialises a new instance of the <see cref="AdminServicesService"/> class.
        /// </summary>
        public AdminServicesService() { }

        #region Properties

        /// <summary>
        /// Gets or sets the event log to write to.
        /// </summary>
        /// <value>The <see cref="EventLog"/> to write to.  May be null to disable logging.</value>
        public EventLog EventLog { get; set; }

        #endregion

        /// <summary>
        /// Returns "Hello world!"
        /// </summary>
        /// <returns>"Hello world!"</returns>
        public string HelloWorld()
        {
            return "Hello world!";
        }

        /// <summary>
        /// Sets the credentials to use.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="domain">The user's domain.</param>
        /// <returns>A log message.</returns>
        public string SetCredential(string username, string password, string domain)
        {
            return string.Format("Received credential for {0}\\{1}", domain, username);
        }
    }
}
