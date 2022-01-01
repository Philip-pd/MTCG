﻿using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace MTCG.SystemLogicClasses
{
    class PlayerDAOImpl : PlayerDAO //Create playerdata class which only has values and doesn't implement actual functions
    {
        public void AddPlayer(string name, string pwd) //take all data from the player
        {
            IDbCommand command = Connect();
            command.CommandText = @"insert into players(name,password)
                values(@name,@pwd)"; //everythhing else is automated
            NpgsqlCommand c = command as NpgsqlCommand; //get automated
            c.Parameters.Add("name", NpgsqlDbType.Varchar, 32);
            c.Parameters.Add("pwd", NpgsqlDbType.Varchar, 64);
            c.Prepare();
            c.Parameters["name"].Value = name;
            c.Parameters["pwd"].Value = pwd;
            command.ExecuteNonQuery(); //somehow check if it worked or check prior if exists
        }

        public void DeletePlayer(string name)
        {
            IDbCommand command = Connect();
            command.CommandText = @"DELETE FROM players
               WHERE name=@name";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.Add("name", NpgsqlDbType.Varchar, 32);
            c.Parameters["name"].Value = name;
            command.ExecuteNonQuery();
        }

        public List<Player> GetAllPlayers() //Maybe just return a json instead cause this is mega wasteful
        {
            throw new NotImplementedException();
        }

        public Player GetPlayerInfo(string name) //still gotta do this
        {
            throw new NotImplementedException();
        }

        public Player GetPlayerLogin(string name,string pwd)
        {
            throw new NotImplementedException();
        }

        public void UpdatePlayer(Player toUpdate) //update literally every value except id,name & pwd
        {
            throw new NotImplementedException();
        }

        public IDbCommand Connect()
        {
            IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=123;Database=MTCG");
            connection.Open();
            IDbCommand command = connection.CreateCommand();

            return command;
            
        }

        
    }
}
//what should this do before moving on?
/*
Create & Delete Player Y
Update Player Stats N

test them both in program N

then build Rest

Then:

Get all players
Get specific player to gen for user
Get specific player for profile view (maybe once getplayer(id) and genplayer(username pwd) )

 */