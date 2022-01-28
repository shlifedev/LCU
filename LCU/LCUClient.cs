using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace LCU
{
    public class LCUClient
    {
        private HttpClient httpClient;
        private WebSocket webSocket;  
        /// <summary>
        /// lcu process info
        /// </summary>
        private LCUProcessInfo processInfo;



        public bool IsConnected { get; set; } 

        /// <summary>
        /// when connected called this.
        /// </summary>
        public event Action OnConnected;

        /// <summary>
        /// when disconnect called this
        /// </summary>
        public event Action OnDisconnected;

   

        /// <summary>
        /// Initialize LCU Client, And Try Connection To LCU Client.
        /// </summary>
        public void Initialize()
        {
            try
            {
                httpClient = new HttpClient(new HttpClientHandler()
                {
                    SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls,
                    ServerCertificateCustomValidationCallback = (a, b, c, d) => true
                });
            }
            catch
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                httpClient = new HttpClient(new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (a, b, c, d) => true
                });
            }


            Task.Delay(2000).ContinueWith(e => TryConnectOrRetry());
            var trytimes = 0;
            while (!IsConnected)
            {
                if (trytimes != 5)
                {
                    trytimes++;
                    TryConnectOrRetry();
                }
                else
                {
                    Console.WriteLine("Connection timed out ");
                    break;
                }
            }
        }

        private void TryConnectOrRetry()
        {
            if (IsConnected) return;
            TryConnect(); 
            Task.Delay(2000).ContinueWith(a => TryConnectOrRetry());
        }

        private void TryConnect()
        {
            try
            {
                if (IsConnected) return;

                var status = LCUClientArgsReader.GetLeagueStatus();
                if (status == null) return;

                var byteArray = Encoding.ASCII.GetBytes("riot:" + status.Port);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                webSocket = new WebSocket("wss://127.0.0.1:" + status.Port + "/", "wamp");
                webSocket.SetCredentials("riot", status.AuthToken, true); 
                webSocket.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12;
                webSocket.SslConfiguration.ServerCertificateValidationCallback = (a, b, c, d) => true;
                webSocket.OnMessage += HandleMessage;
                webSocket.OnClose += HandleDisconnect;
                webSocket.Connect();
                webSocket.Send($"[5, \"OnJsonApiEvent\"]");

                processInfo = status;
                IsConnected = true; 
                OnConnected?.Invoke(); 
                Console.WriteLine("Connected To LCU Client");
            }
            catch (Exception e)
            {
                processInfo = null;
                IsConnected = false; 
            }
        }
         
        private void HandleMessage(object sender, MessageEventArgs args)
        {
            if (!args.IsText) return;
            Console.WriteLine(args.RawData);
        }

        private void HandleDisconnect(object sender, CloseEventArgs args)
        {
            Console.WriteLine("disconnected");
        }
    }
}
