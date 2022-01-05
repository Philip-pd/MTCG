using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.SystemLogicClasses
{
    public sealed class PlayerHandler
    {
        private static PlayerHandler instance = null;
        private static readonly object padlock = new object();
        public List<Player> PlayersOnline = new List<Player>(); //might be a problem if I just keep creating them //could add logout and then write in specs that if actually implemented remove players after certain inactivity time
        public List<Player> PlayersInMM = new List<Player>();

        PlayerHandler()
        {
        }

        public static PlayerHandler Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new PlayerHandler();
                    }
                    return instance;
                }
            }
        }
        public bool PlayerLogin(Player toAdd)
        {
            lock (padlock)
            {
                for (int i = 0; i < PlayersOnline.Count; i++)
                {
                    if (PlayersOnline[i].Name == toAdd.Name)
                        return false;
                }
                this.PlayersOnline.Add(toAdd);
            }
            return true;
        }

        public Player GetPlayerOnline(string token)  
        {
            lock (padlock)
            {
                foreach (Player player in PlayersOnline)
                {
                    if (player.Token == token)
                        return player;
                }
            }
            return null;
        }

        public Player GetPlayerMatchmaking(string token)
        {
            lock (padlock)
            {
                foreach (Player player in PlayersInMM)
                {
                    if (player.Token == token)
                        return player;
                }
            }
            return null;
        }

        public bool PlayerLogout(string token)
        {
            lock (padlock)
            {
                foreach (Player player in PlayersOnline) //remove you from list of online Players
                {
                    if (player.Token == token)
                    {
                        PlayersOnline.Remove(player);
                        return true;
                    }
                }
            }
            return false;
        }
        public bool RemovefromQueue(string token)
        {
            lock (padlock)
            {
                foreach (Player player in PlayersInMM) //in case you are still Queued remove you from queue
                {
                    if (player.Token == token)
                    {
                        PlayersInMM.Remove(player);
                        return true;
                    }
                }
            }

            return false;
        }
        public string PlayerEnterMM(string token) //test this multithreaded
        {
            Player toAdd = this.GetPlayerOnline(token); //get from logged in list
            if (toAdd == null)
                return "error";
            Battle battle = null;
            lock (padlock) //needs to lock as it is critical code about order of operations
            {
                foreach (Player player in PlayersInMM) //check if already in MM
                {
                if (player.Token == token)
                    return "error";
                }
                if(toAdd.Deck[0]==-1)
                    return "InvalidDeck";
                PlayersInMM.Add(toAdd); //Add to MM
                
            
                if (PlayersInMM.Count<2)
                return "Player Successfully entered Queue. Waiting for Opponent...";
                battle = new Battle(PlayersInMM[0], PlayersInMM[1]);
                PlayersInMM.RemoveAt(0);
                PlayersInMM.RemoveAt(0);
            }
            return battle.Play();
        }
    }
    
}
