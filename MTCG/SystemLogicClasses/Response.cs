using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace MTCG.SystemLogicClasses
{
    public class Response
    {
        private Byte[] data = null;
        private string status;
        private string mime;
         
        private Response(string status, string mime, Byte[] data) //Constructor only meant to be called from self
        {
            this.data = data;
            this.status = status;
            this.mime = mime;
            
        }

        public static Response From(Request request) //Generates actual Response
        {
            if (request == null) //if no Request allocated
                return MakeNullRequest();

            if (request.Type == "GET") 
            {
                String[] info = request.URL.Split('?', '=', '&'); //Get GET Parametres out of URL
                switch (info[0]) //Look where it wants to go
                {
                    case "/demo":
                        System.Threading.Thread.Sleep(10000); //Just to test multithreadedness.
                        return MakeNullRequest(); //400
                    default:
                        return MakePageNotFound(); //404
                }

            }
            else if (request.Type == "POST") //POST parametres are in request parametres and URL doesn't need to be edited
            {
                switch (request.URL)
                {
                    case "/Player":
                        if (request.parametres.Length < 4) //if too few parametres
                            return MakeNullRequest();
                        return MakePlayer(request.parametres[1], request.parametres[3]); //right ones
                    default:
                        return MakePageNotFound();
                }
            }
            else
            {
                return MakeMethodNotAllowed();
            }
        }

        private static Response MakePlayer(string name, string pwd) //remove just used for tests
        {
            PlayerDAO dao = new PlayerDAOImpl();
            if (name.Length > 32 || pwd.Length > 64 || !dao.AddPlayer(name, pwd))
                return ResetContentRequest();            
            return ShowPlayerJSON(name);
        }

        private static Response ShowPlayerJSON(string name)
        {
            PlayerDAO dao = new PlayerDAOImpl();
            Player temp = dao.GetPlayerInfo(name);
            if (temp == null)
                return MakePageNotFound();
            string returntext = JsonConvert.SerializeObject(temp);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("200 OK", "application/json", d);
        }

        private static Response ResetContentRequest()
        {
            string returntext="Invalid Parametres"; //client would then based on context check if entry too long or invalid password
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("205 Reset Content", "text/html", d);
        }

        private static Response MakeNullRequest() //400
        {
            string returntext = "Bad Request"; 
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("400 Bad Request", "text/html", d);
        }

        private static Response MakePageNotFound() //404
        {
            string returntext = "Doesn't Exist"; 
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("404 Page Not Found", "text/html", d);
        }

        private static Response MakeMethodNotAllowed() //405
        {
            string returntext = "Unsupported Method";
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("405 Method Not Allowed", "text/html", d);
        }

        public void Post(NetworkStream stream) //returns to Client
        {
            StreamWriter writer = new StreamWriter(stream);

            writer.WriteLine(string.Format("{0} {1}\r\nServer: {2}\r\nContent-Type: {3}\r\nAccept-Ranges\r\nContent-Length: {4}\r\n",
                HTTPServer.VERSION, status, HTTPServer.NAME, mime, data.Length));
            writer.Flush();
            stream.Write(data, 0, data.Length);
        }
    }
}
//overall get rid of all the file things