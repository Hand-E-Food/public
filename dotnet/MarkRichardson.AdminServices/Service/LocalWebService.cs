using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Threading;

namespace MarkRichardson.AdminServices
{

    /// <summary>
    /// The automation service.
    /// </summary>
    partial class LocalWebService : ServiceBase
    {

        /// <summary>
        /// Initialises a new instance of the ExperimentService class.
        /// </summary>
        public LocalWebService()
        {
            InitializeComponent();
            AdminServicesService = new AdminServicesService() { EventLog = this.EventLog };
        }

        #region Constants ...

        /// <summary>
        /// The base address for hosted web services.
        /// </summary>
        public static readonly string BaseAddress = "http://localhost:8000/";

        #endregion

        #region Properties ...

        /// <summary>
        /// The implementation of the administration operations.
        /// </summary>
        private AdminServicesService AdminServicesService { get; set; }

        /// <summary>
        /// The service host for the web service.
        /// </summary>
        private ServiceHost WebServiceHost { get; set; }
        
        #endregion

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        internal void Start(string[] args)
        {
            OnStart(args);
            EventLog.WriteEntry("Service started successfully.", EventLogEntryType.Information);
        }
        /// <summary>
        /// Executes when a Start command is sent to the service by the Service Control Manager (SCM) or
        /// when the operating system starts (for a service that starts automatically).  Specifies actions
        /// to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            StartWebService();
            StartKeepAlive();
        }
        /// <summary>
        /// Starts the web service.
        /// </summary>
        private void StartWebService()
        {
            WebServiceHost = new WebServiceHost(AdminServicesService, new Uri(BaseAddress));
            try
            {
                WebServiceHost.AddServiceEndpoint(typeof(IAdminServices), new WebHttpBinding(), "");
                WebServiceHost.Open();
            }
            catch (CommunicationException ex)
            {
                WebServiceHost.Abort();
                throw;
            }
        }
        /// <summary>
        /// Starts the KeepAlive thread.
        /// </summary>
        private void StartKeepAlive()
        {
            keepAliveWaitHandle.Reset();
            keepAliveThread = new Thread(KeepAlive) { Name = typeof(LocalWebService).FullName + ".KeepAlive" };
            keepAliveThread.Start();
        }
        
        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the
        /// Service Control Manager (SCM).  Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            StopKeepAlive();
            StopWebService();
        }
        /// <summary>
        /// Stops the KeepAlive thread.
        /// </summary>
        private void StopKeepAlive()
        {
            keepAliveWaitHandle.Set();
            keepAliveThread.Join();
        }
        /// <summary>
        /// Stops the web service.
        /// </summary>
        private void StopWebService()
        {
            WebServiceHost.Close();
        }
        
        /// <summary>
        /// Keeps the service alive.
        /// </summary>
        private void KeepAlive()
        {
            keepAliveWaitHandle.WaitOne();
        }
        /// <summary>
        /// The thread that is keeping the service alive.
        /// </summary>
        private Thread keepAliveThread = null;
        /// <summary>
        /// The wait handle used to terminate the KeepAlive thread.
        /// </summary>
        private EventWaitHandle keepAliveWaitHandle = new ManualResetEvent(false);
    }
}
