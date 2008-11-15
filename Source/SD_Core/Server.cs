using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;

namespace SD.Core
{
    internal class Server
    {
        HttpListener _listener;
        string _baseFolder;

        public Server (string uriPrefix, sting baseFolder)
        {
            System.Threading.ThreadPool.SetMaxThreads(5, 1000);
            System.Threading.ThreadPool.SetMinThreads(5, 5);
            _listener = new HttpListener();
            _listener.Prefixes.Add(uriPrefix);
            _baseFolder = baseFolder;
        }

        public void Start()
        {
            _listener.Start();
            while(true)
                try
                {
                    HttpListenerContext request = _listener.GetContext();
                    ThreadPool.QueueUserWorkItem (ProcessRequest, request);
                }
            catch (HttpListenerException)     { break; }
            catch (InvalidOperationException) { break; }
        }

        public void Stop() { _listener.Stop(); }

        void ProcessRequest(object listenerContext)
        {
            try
            {
                var context = (HttpListenerContext) listenerContext;
                string filename = Path.GetFileName (context.Request.RawUrl);
                string path = path.Combine (_baseFolder, filename);
                byte[] msg;
                if (!File.Exists(path))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    msg = Encoding.UTF8.GetBytes("No sir, doesn't exist");
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    msg.ReadAllBytes(path);
                }
                context.Response.ContentLength64 = msg.Length;
                using (Stream s = context.Response.OutputStream)
                    s.Write(msg, o, msg.Length);
            }
            catch (Exception ex) { Console.WriteLine ("Request error: " + ex);
        }

    }
}
