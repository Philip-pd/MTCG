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
            for(int i = 0; i<PlayersOnline.Count;i++)
            {
                if (PlayersOnline[i].Name == toAdd.Name)
                    return false;
            }
            this.PlayersOnline.Add(toAdd);
            return true;
        }

        public Player GetPlayerOnline(string token)
        {
            foreach(Player player in PlayersOnline)
            {
                if (player.Token == token)
                    return player;
            }
            return null;
        }

        public Player GetPlayerMatchmaking(string token)
        {
            foreach (Player player in PlayersInMM)
            {
                if (player.Token == token)
                    return player;
            }
            return null;
        }
    }
}
