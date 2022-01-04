using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace MTCG.SystemLogicClasses
{
    public class HTTPServer
    {
        public const string MSG_DIR = "/root/msg/"; //probs remove later for now just here
        public const string WEB_DIR = "/root/web/";
        public const string VERSION = "HTTP/1.1";
        public const string NAME = "MTCG-Server Test";


        private bool running = false;

        private TcpListener listener;

        public HTTPServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start() 
        {
            Thread serverThread = new Thread(new ThreadStart(Run)); //splits of thread to run server in
            serverThread.Start();
        }
        private void Run() //starts the server
        {
            running = true;
            listener.Start();
            while (running)
            {
                Console.WriteLine("Waiting for connection...");
                TcpClient client = listener.AcceptTcpClient(); //generate client listener

                Console.WriteLine("Client connected!");
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient)); //prepare thread
                t.Start(client); //split of thread for client


            }

            running = false;
            listener.Stop(); //server end
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj; //get back parametrized obj
            StreamReader reader = new StreamReader(client.GetStream()); //get clients requests

            String msg = "";
            while (reader.Peek() != -1) //checks if there is more
            {
                msg += reader.ReadLine() + "\n"; //needs to break if there is a body
            }
            //if body get body with function
            Debug.WriteLine("Request:\n" + msg); //writes Request so I can see what's going on

            Request req = Request.GetRequest(msg); //generates request from string //add body as second param here
            Response resp = Response.From(req); //generates response based on request
            resp.Post(client.GetStream()); //returns data to client.
            client.Close(); //closes client
        }
    }
}
