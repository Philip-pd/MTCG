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
                    case "/Demo":
                        System.Threading.Thread.Sleep(10000); //Just to test multithreadedness.
                        return MakeNullRequest(); //400
                    case "/Ranking": //List of players based on elo
                        return MakePlayerList();
                    case "/Player": //?name=name <-- get that also no need to be logged in; 1
                        return ShowPlayerJSON(info[2]); //?name= info[2]
                    case "/Collection": //just returns own collection
                        return MakeOwnCollection(request.parametres[1]); //parameter 1 is token
                    case "/Trades": //List of all trades that are currently open 3
                        break;
                    case "/Packs": //gives Pack info 2
                        break;
                    default:
                        return MakePageNotFound(); //404
                }
                return MakePageNotFound(); //remove later
            }
            else if (request.Type == "POST") //POST parametres are in request parametres and URL doesn't need to be edited
            {
                switch (request.URL)
                {
                    case "/Player":
                        if (request.parametres.Length < 4) //if too few parametres
                            return MakeNullRequest();
                        return MakePlayer(request.parametres[1], request.parametres[3]); //right ones
                    case "/Login":
                        if (request.parametres.Length < 4) //if too few parametres
                            return MakeNullRequest();
                        return LoginPlayer(request.parametres[1], request.parametres[3]); //right ones
                    case "/Trade": //if only 1 param it accepts that trade else it makes its own 3
                        break;
                    case "/Pack": //Buys Pack 1
                        break;
                    case "/Battle": //Joins MM 4
                        break;
                    case "/Logout": //Removes from PlayerOnline
                        break;

                    default:
                        return MakePageNotFound();
                }
                return MakePageNotFound(); //remove later
            }
            else
            {
                return MakeMethodNotAllowed();
            }
        }

        

        private static Response MakePlayerList()
        {
            PlayerDAO dao = new PlayerDAOImpl();
            List<Player> players = dao.GetAllPlayers();
            string returntext = JsonConvert.SerializeObject(players, Formatting.Indented);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("200 OK", "application/json", d);
        }

        //------Good Responses ----------//

        private static Response LoginPlayer(string name, string password)
        {
            PlayerDAO dao = new PlayerDAOImpl();
            Player LoggedIn = dao.GetPlayerLogin(name, password);
            if (LoggedIn == null)
                return ResetContentRequest();
            PlayerHandler handler = PlayerHandler.Instance;
            if (!handler.PlayerLogin(LoggedIn))
                return MakeAlreadyLoggedIn();
            string returntext = LoggedIn.Token;
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("200 OK", "text/html", d); //returns just the token so client knows what to use now
            //if logged in already return  409 Conflict - This Player is already logged in
        }

        private static Response MakeOwnCollection(string token)
        {
            PlayerHandler handler = PlayerHandler.Instance;
            Player player = handler.GetPlayerOnline(token);
            if (player == null)
                return NotLoggedIn();
            string returntext = player.ReturnCollection();
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("200 OK", "application/json", d);
        }

        private static Response MakePlayer(string name, string pwd) //remove just used for tests
        {
            PlayerDAO dao = new PlayerDAOImpl();
            if (name.Length > 32 || pwd.Length > 64 || !dao.AddPlayer(name, pwd))
                return ResetContentRequest();            
            return ShowNewPlayerJSON(name);
        }

        private static Response ShowNewPlayerJSON(string name)
        {
            PlayerDAO dao = new PlayerDAOImpl();
            Player temp = dao.GetPlayerInfo(name);
            if (temp == null)
                return MakePageNotFound();
            string returntext = JsonConvert.SerializeObject(temp);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("201 Created", "application/json", d);
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

        //----------ERROR CODES-------------//

        private static Response ResetContentRequest() //205
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
            
            private static Response NotLoggedIn() //403
            {
                string returntext = "Login Required"; 
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                Byte[] d = enc.GetBytes(returntext);
                return new Response("403 Forbidden", "text/html", d);
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

            private static Response MakeAlreadyLoggedIn() //409
            {
            string returntext = "Already Logged In";
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("409 Conflict", "text/html", d);
            }

        //--Send back to Client--//

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