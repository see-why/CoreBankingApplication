using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBA.FEP.MessageFields;
using Trx.Messaging.Channels;
using Trx.Messaging.FlowControl;
using Trx.Messaging.Iso8583;
using System.IO;
using Trx.Messaging;

namespace CBA.FEP.SwitchTransaction
{
    public class ConnectionClient
    {
        private ListenerPeer _listenerPeer;
        private int _requestsCnt = 0;
        string localInterface = "";
        private int port = 0;

        public ConnectionClient()
        {
            try
            {
                //localInterface = System.Configuration.ConfigurationManager.AppSettings["AcquirerHostname"];
                localInterface = System.Configuration.ConfigurationManager.AppSettings["AcquirerIP"];
                port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AcquirerPort"]);
            }
            catch
            {
                localInterface = "127.0.0.1";
                port = 2021;
            }
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(port);
            //TODO: TCPlistener was not started
            listener.LocalInterface = localInterface;
            listener.Start();
            _listenerPeer = new ListenerPeer("Acquirer", new TwoBytesNboHeaderChannel(
                new Iso8583Ascii1987BinaryBitmapMessageFormatter(), localInterface, port),
                    new BasicMessagesIdentifier(11, 41), listener);
            Log("FEP: " + _listenerPeer.Name + " listening at " + localInterface + " on " + port);

            //_listenerPeer = new ListenerPeer("Acquirer", new TwoBytesNboHeaderChannel(
            //    new Iso8583Ascii1987BinaryBitmapMessageFormatter()), listener);

            _listenerPeer.Receive += new PeerReceiveEventHandler(OnReceive);
            _listenerPeer.Connected += new PeerConnectedEventHandler(_listenerPeer_Connected);
            _listenerPeer.Disconnected += new PeerDisconnectedEventHandler(listenerPeerDisconnected);
           // _listenerPeer.Connect();
        }

        private void listenerPeerDisconnected(object sender, EventArgs e)
        {
            ListenerPeer peer = sender as ListenerPeer;
            if (peer == null) return;
            Log("Switch disconnected from =/=> " + peer.Name, ConsoleColor.Yellow);
           // Stop();
            Start();
        }

        void _listenerPeer_Connected(object sender, EventArgs e)
        {
            ListenerPeer peer = sender as ListenerPeer;
            if (peer == null) return;
            Log("Connected to ==> " + peer.Name, ConsoleColor.Green);
            if (_requestsCnt==0)//only send when connected for the first time
            {
                //  on connection, just send back an echo message
                Iso8583Message responseMsg = new Iso8583Message(800);
                DateTime transmissionDate = DateTime.Now;
                responseMsg.Fields.Add(FieldNos.F7_TransDateTime, string.Format("{0}{1}",
                    string.Format("{0:00}{1:00}", transmissionDate.Month, transmissionDate.Day),
                    string.Format("{0:00}{1:00}{2:00}", transmissionDate.Hour,
                    transmissionDate.Minute, transmissionDate.Second)));
                responseMsg.Fields.Add(FieldNos.F11_Trace, new Random().Next(10).ToString().PadLeft(6, '0'));//_sequencer.Increment().ToString() );
                responseMsg.Fields.Add(FieldNos.F12_TransLocalTime, string.Format("{0:00}{1:00}{2:00}", transmissionDate.Hour,
                    transmissionDate.Minute, transmissionDate.Second));
                responseMsg.Fields.Add(FieldNos.F13_TransLocalDate, string.Format("{0:00}{1:00}", transmissionDate.Month, transmissionDate.Day));
                responseMsg.Fields.Add(FieldNos.F70_NetworkMgtInfoCode, "301");
                peer.Send(responseMsg);
            }
                       
           
        }

        private void OnReceive(object sender, ReceiveEventArgs e)
        {
            ListenerPeer sourcePeer = sender as ListenerPeer;
            _requestsCnt++;

            Iso8583Message IsoMessage = e.Message.Clone() as Iso8583Message;
            if (IsoMessage.Fields[90].ToString().Length > 23)
            {
                IsoMessage.Fields.Add(90, IsoMessage.Fields[90].ToString().Remove(0, 23));
            }
            //IsoMessage.Fields.Add(90,IsoMessage.Fields[90].ToString().Remove(0, 23));

            if (IsoMessage != null)
            {
                var cardProcessor = new CardTransactionProcessor();

                try
                {
                    cardProcessor.LogCardTrxnToCBA(IsoMessage);

                    if (IsoMessage.IsNetworkManagement())
                    {
                        Log("Network Management message Received...", ConsoleColor.Blue);
                    }
                    else if (IsoMessage.IsReversalOrChargeBack())
                    {
                        Log("Reversal message Received...", ConsoleColor.Blue);
                        IsoMessage = cardProcessor.ProcessReversal(IsoMessage);
                    }
                    else
                    {
                        //Process Balance inquiry or withdrawal
                        IsoMessage = cardProcessor.ProcessCardTransaction(IsoMessage);
                    }

                    // Build the response based in the received message.
                    IsoMessage.SetResponseMessageTypeIdentifier();
                    //IsoMessage.Fields.Add(FieldNos.F39_ResponseCode, "00");
                    
                    cardProcessor.LogCardTrxnToCBA(IsoMessage);
                }
                catch (Exception ex)
                {
                    //not successfull
                    //new PANE.ERRORLOG.Error().LogToFile(ex);
                    int expt = 06;
                    int.TryParse(ex.Message.Split(':')[0], out expt);
                    IsoMessage.Fields.Add(FieldNos.F39_ResponseCode, expt.ToString());
                    
                    cardProcessor.LogCardTrxnToCBA(IsoMessage);
                }

                try
                {
                    sourcePeer.Send(IsoMessage);
                }
                catch (Exception)
                {
                                        
                }

                sourcePeer.Close();
               // sourcePeer.Dispose();

            }
        }

        
        /// <summary>
        /// Returns the number of requests made.
        /// </summary>
        public int RequestsCount
        {

            get
            {

                return _requestsCnt;
            }
        }

        /// <summary>
        /// Stop acquirer activity.
        /// </summary>
        public void Stop()
        {
            _listenerPeer.Close();
            _listenerPeer.Dispose();
        }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            ConnectionClient a = new ConnectionClient();
            try
            {
                // Initializes log4net.
                //LogManager.GetLogger("root");
                string localInterface = System.Configuration.ConfigurationManager.AppSettings["AcquirerHostname"];
                if (args.Length > 0)
                {
                    localInterface = args[0];
                }
                a.Start();
                Log("CBA FEP is running, press any key to stop it...");
                Console.ReadLine();
                //a.Stop();
                Log(string.Format("Requests processed: {0}", a.RequestsCount));
                Log("Press any key to exit...");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
            }
            finally 
            {
                a.Stop();
            }

        }
        public static void Log(String msg, ConsoleColor consoleColor = ConsoleColor.White)
        {
            Console.ForegroundColor = consoleColor;
            //System.Diagnostics.Trace.TraceWarning(msg);
            try
            {
                using (StreamWriter LogWriter = new StreamWriter(@"G:\Visual Studio 2013\Projects\CBA\LogFiles\CBA_FEP_logs.txt", true))
                {
                    LogWriter.WriteLine(msg + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + Environment.NewLine);
                }
            }
            catch (Exception)
            {                                
            }
            Console.WriteLine("\n " + msg + " " + DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt") + Environment.NewLine);
        }
    }
}
