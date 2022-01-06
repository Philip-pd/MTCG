using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.SystemLogicClasses
{
    interface PlayerDAO
    {
        List<Player> GetAllPlayers();
        Player GetPlayerLogin(string name,string pwd); 
        Player GetPlayerInfo(string name);
        bool AddPlayer(string name, string pwd); 
        void UpdatePlayer(Player toUpdate);
        void DeletePlayer(string name);
        void UpdatePlayerDeck(Player player);
    }
}
