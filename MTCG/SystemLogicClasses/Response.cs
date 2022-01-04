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
                    case "/Player": //?name=name <-- get that also no need to be logged in; 
                        return ShowPlayerJSON(info[2]); //?name= info[2]
                    case "/Collection": //just returns own collection
                        return MakeOwnCollection(request.parametres[1]); //parameter 1 is token
                    case "/Trades": //List of all trades that are currently open 
                        return GetTrades();
                    case "/Pack": //gives Pack info 
                        return ShowPackInfo(info[2]); //?id= info[2]
                    case "/Battle": //Instantly sent by client after entering MM and no imediate battle. Waits for Battle Results
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
                        return MakePlayer(request.parametres[1], request.parametres[3]); //1=name, 3=pwd
                    case "/Login":
                        if (request.parametres.Length < 4) //if too few parametres
                            return MakeNullRequest();
                        return LoginPlayer(request.parametres[1], request.parametres[3]); //1=name, 3=pwd
                    case "/Trade": //1=token //if only token either accept trade or cancel if own // 3= card offered, 5 = card wanted 7 = coins wanted;
                        return ManageTrade(request.parametres);
                    case "/Pack": //Buys Pack 
                        return BuyPack(request.parametres[1], request.parametres[3]); //1=token, 3=packID
                    case "/EnterMM": //Joins MM  //returns more to come or if already 1 inside gets battle results
                        return EnterMM(request.parametres[1]);
                    case "/Logout": //Removes from PlayerOnline
                        return LogOutPlayer(request.parametres[1]);
                    case "/Deck": //makes a deck takes 5 params. Token and 4 fields for deck
                        return UpdateDeck(request.parametres[1], request.parametres[3]); //1=token, 3=cards
                    default:
                        return MakePageNotFound();
                }
            }
            else
            {
                return MakeMethodNotAllowed();
            }
        }

        private static Response ManageTrade(string[] parametres)
        {
            string returntext = "Server Exception";
            string code = "500 Internal Server Error";
            string type = "text / html";
            PlayerHandler handler = PlayerHandler.Instance;
            Player player = handler.GetPlayerOnline(parametres[1]);
                
            int tradeid = 0;
            if (player == null) //if player not logged in return that
                return NotLoggedIn();
            string[] tradeparams = parametres[3].Split(',');
            if (tradeparams.Length==1) //split
            {
                if (!Int32.TryParse(tradeparams[0], out tradeid)) //if invalid tradeID cancel
                    ResetContentRequest();
                TradeDAO tdao = new TradeDAOImpl(); //tradedao gets the trade
                Trade trade = tdao.GetTrade(tradeid); 
                if(trade==null) //if the trade doesn't exist send error
                    return MakeNullRequest();

                if(trade.Owner==player.Name) //if trade ownder sent the request Cancel it
                {
                    if (tdao.CancelTrade(player, trade.ID))
                    {
                        returntext = "Success Trade Canceled";
                        code = "200 OK";
                    }
                } else
                {
                    if(tdao.AcceptTrade(player, trade.ID)) //if you can accept the trade
                    {
                        returntext = "Trade was a Success";
                        code = "200 OK";
                    }else //else say it didn't work
                    {
                        returntext = "You can't accept that Trade";
                        code = "403 Forbidden";
                    }
                }
            } else if(tradeparams.Length==3)  // 1= card offered, 2 = card wanted 3 = coins wanted;
            {
                int p0, p1, p2;
                if (Int32.TryParse(tradeparams[0], out p0) && Int32.TryParse(tradeparams[1], out p1) && Int32.TryParse(tradeparams[2], out p2))
                {
                    Trade trade = new Trade(0, player.Name, p0, p1, p2); //creates trade based on parameters
                    TradeDAO tdao = new TradeDAOImpl(); //tradedao gets the trade
                    if(tdao.CreateTrade(player,trade)) //if successfully created
                    {
                        returntext = JsonConvert.SerializeObject(trade);
                        code = "200 OK";
                        type = "application/json";
                    } else
                    {
                        returntext = "You can't create that Trade";
                        code = "403 Forbidden";
                    }
                        
                }else //if not integers
                {
                    ResetContentRequest();
                }
            }

            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response(code, type, d); //any unspecified amount of params

        }


        //------Good Responses ----------//

        private static Response EnterMM(string token)
        {
            string code= "200 OK";
            PlayerHandler handler = PlayerHandler.Instance;
            if (handler.GetPlayerOnline(token)==null) //Checks here as well for different error code
                return NotLoggedIn();
            string returntext = handler.PlayerEnterMM(token);
            if (returntext == "error")
                return AlreadyQueued();
            if (returntext == "InvalidDeck")
                code = "403 Forbidden";
            if (returntext== "Player Successfully entered Queue. Waiting for Opponent...") //starts with and add integer for retrieval back
            {
                code = "202 Accepted";//Client imediateley sends GET Battle request after
            } //add if 2 queue for instant info
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response(code, "text/html", d);


        }
        private static Response LoginPlayer(string name, string password)
        {
            PlayerDAO dao = new PlayerDAOImpl();
            Player LoggedIn = dao.GetPlayerLogin(name, password); //check if player could log in
            if (LoggedIn == null)
                return ResetContentRequest();
            PlayerHandler handler = PlayerHandler.Instance; //use playerhandler for state management
            if (!handler.PlayerLogin(LoggedIn)) //check if player is already logged in
                return MakeAlreadyLoggedIn();
            string returntext = LoggedIn.Token; //return token
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("200 OK", "text/html", d); //returns just the token so client knows what to use now
            //if logged in already return  409 Conflict - This Player is already logged in
        }

        private static Response LogOutPlayer(string token)
        {
            PlayerHandler handler = PlayerHandler.Instance;
            if (!handler.PlayerLogout(token)) //check if Logged in
                return NotLoggedIn();
            string returntext = "Logout Successful";
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("200 OK", "text/html", d);
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

        private static Response MakePlayerList()
        {
            PlayerDAO dao = new PlayerDAOImpl();
            List<Player> players = dao.GetAllPlayers();
            string returntext = JsonConvert.SerializeObject(players, Formatting.Indented);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("200 OK", "application/json", d);
        }

        private static Response ShowPackInfo(string packID)
        {
            int pack = 0;
            if (!Int32.TryParse(packID, out pack) || pack < 0 || pack > 10) //check if pack exists
                return MakeNullRequest();
            BoosterPack booster = new BoosterPack();
            string returntext = JsonConvert.SerializeObject(booster.GetPack(pack));
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("200 OK", "application/json", d);
        }
        private static Response BuyPack(string token, string packID)
        {
            int pack = 0;
            PlayerHandler handler = PlayerHandler.Instance;
            Player player = handler.GetPlayerOnline(token);
            if (player == null) //check if Logged in
                return NotLoggedIn();
            if (player.Coins < 5)
                return InsufficientFunds();
            if (!Int32.TryParse(packID, out pack) || pack < 0 || pack > 10) //check if pack exists
                return MakeNullRequest();
            BoosterPack booster = new BoosterPack();
            int[] newCards = booster.GetPack(pack);
            int added = player.AddBoosterToCollection(newCards);
            string returntext = $"{{\r\"NewCardsAdded\": { added},\r\"CoinsRefunded\": {5 - added},\"Collection\":\r{player.ReturnCollection()}}}";
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("200 OK", "application/json", d);
        }

        private static Response UpdateDeck(string token, string content)
        {
            string[] deckstrings = content.Split(',');
            int[] deckints = new int[4];
            if (deckstrings.Length != 4)
                return MakeNullRequest();
            PlayerHandler handler = PlayerHandler.Instance;
            Player player = handler.GetPlayerOnline(token);
            if (player == null)
                return NotLoggedIn();
            for (int i = 0; i < deckstrings.Length; i++)
            {
                if (!Int32.TryParse(deckstrings[i], out deckints[i]))
                    return MakeNullRequest();
            }
            if (!player.CreateDeck(deckints))
                return NotInCollection();
            string returntext = JsonConvert.SerializeObject(player.Deck);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            Byte[] d = enc.GetBytes(returntext);
            return new Response("200 OK", "application/json", d);
        }

        private static Response GetTrades()
        {
            TradeDAO dao = new TradeDAOImpl();
            List<Trade> trades = dao.GetAllTrades();
            string returntext = JsonConvert.SerializeObject(trades, Formatting.Indented);
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

            private static Response NotInCollection() //403
            {
                string returntext = "Error: One or More Cards You selected aren't part of your collection";
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                Byte[] d = enc.GetBytes(returntext);
                return new Response("403 Forbidden", "text/html", d);
            }

            private static Response InsufficientFunds() //403
            {
                string returntext = "Isufficient Funds";
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

            private static Response AlreadyQueued() //409
            {
                string returntext = "Already In Queue";
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
