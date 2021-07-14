using CBA.FEP.SwitchTransaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CBA.FEPService
{
    partial class FEPService : ServiceBase
    {
        ConnectionClient conClient = new ConnectionClient();

        public FEPService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                //string localInterface = System.Configuration.ConfigurationManager.AppSettings["AcquirerHostname"];
                //if (args.Length > 0)
                //{
                //    localInterface = args[0];
                //}

                conClient.Start();
                Console.WriteLine("CBA FEP is running, press any key to stop it...");
                Console.ReadLine();
                LogToFile("CBA FEP is running, press any key to stop it...");
                //a.Stop();

                Console.WriteLine(string.Format("Requests processed: {0}", conClient.RequestsCount));
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                LogToFile(string.Format("Requests processed: {0}", conClient.RequestsCount));


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error starting FEP Service: " + ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
                LogToFile("Error starting FEP Service: " + ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                conClient.Stop();
                Console.WriteLine("FEP Service stopped...");
                LogToFile("FEP Service stopped");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error stopping FEP Service: " + ex.Message);
                LogToFile("Error stopping FEP Service: " + ex.Message);
            }
        }

        public void LogToFile(string msg)
        {
            System.Diagnostics.Trace.TraceWarning(msg);
            using (StreamWriter LogWriter = new StreamWriter(@"G:\Visual Studio 2013\Projects\CBA\CBA.FEPService\FEPWindowsServiceLogs.txt", true))
            {
                LogWriter.WriteLine(msg + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + Environment.NewLine);
            }
        }
    }
}
