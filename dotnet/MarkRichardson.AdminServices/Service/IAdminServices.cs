using System.ServiceModel;
using System.ServiceModel.Web;

namespace MarkRichardson.AdminServices
{

    /// <summary>
    /// The available operations for <see cref="MarkRichardson.AdminServices"/>.
    /// </summary>
    [ServiceContract(Namespace = "http://MarkRichardson.AdminServices")]
    public interface IAdminServices
    {

        /// <summary>
        /// Returns "Hello world!"
        /// </summary>
        /// <returns>"Hello world!"</returns>
        [OperationContract]
        [WebGet]
        string HelloWorld();

        /// <summary>
        /// Sets the credentials to use.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="domain">The user's domain.</param>
        /// <returns>A log message.</returns>
        [OperationContract]
        [WebGet]
        string SetCredential(string username, string password, string domain);
    }
}
