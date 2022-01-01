using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.SystemLogicClasses
{
    interface PlayerDAO
    {
        List<Player> GetAllPlayers();
        Player GetPlayerLogin(string name,string pwd); //or string token //then check in rest if 0 got returned and if so then write pwd wrong; Still need to somehow get a player when just looking at profile
        Player GetPlayerInfo(string name);
        void AddPlayer(string name, string pwd); //change to bool and if didn't work then send error
        void UpdatePlayer(Player toUpdate);
        void DeletePlayer(string name);
        

    }
}
