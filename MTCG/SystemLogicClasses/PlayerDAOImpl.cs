using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace MTCG.SystemLogicClasses
{
    class PlayerDAOImpl : PlayerDAO //Create playerdata class which only has values and doesn't implement actual functions
    {
        public bool AddPlayer(string name, string pwd) //take all data from the player
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
            try
            {

            command.ExecuteNonQuery(); //somehow check if it worked or check prior if exists
                return true;
            } catch(NpgsqlException)
            {
                return false;
            }
        }

        public void DeletePlayer(string name)
        {
            IDbCommand command = Connect();
            command.CommandText = @"DELETE FROM players
               WHERE name=@name";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.Add("name", NpgsqlDbType.Varchar, 32);
            c.Prepare();
            c.Parameters["name"].Value = name;
            command.ExecuteNonQuery();
        }

        public List<Player> GetAllPlayers() //Returns List of All existing players
        {
            List<Player> players = new List<Player>();

            IDbCommand command = Connect();
            command.CommandText = @"SELECT * FROM players ORDER BY elo DESC";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Prepare();
            NpgsqlDataReader dr = c.ExecuteReader();
            while (dr.Read())
            {
                //0-name, 1-pwd, 2-coins, 3-collection, 4-elo, 5-win, 6-loss
                Player toAdd = new Player((string)dr[0], (int)dr[4], (int)dr[2], (int)dr[3], (int)dr[5], (int)dr[6]);
                players.Add(toAdd);
            }
            
            return players;
        }

        public Player GetPlayerInfo(string name) //still gotta do this
        {
            IDbCommand command = Connect();
            command.CommandText = @"SELECT * FROM players
               WHERE name=@name";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.Add("name", NpgsqlDbType.Varchar, 32);
            c.Prepare();
            c.Parameters["name"].Value = name;
            NpgsqlDataReader dr = c.ExecuteReader();
            if(dr.Read())
            { 
                //0-name, 1-pwd, 2-coins, 3-collection, 4-elo, 5-win, 6-loss
            return new Player((string)dr[0],(int)dr[4], (int)dr[2], (int)dr[3], (int)dr[5], (int)dr[6]);
            }
            return null;
        }

        public Player GetPlayerLogin(string name,string pwd)
        {
            IDbCommand command = Connect();
            command.CommandText = @"SELECT * FROM players
               WHERE name=@name AND password=@pwd";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.Add("name", NpgsqlDbType.Varchar, 32);
            c.Parameters.Add("pwd", NpgsqlDbType.Varchar, 64);
            c.Prepare();
            c.Parameters["name"].Value = name;
            c.Parameters["pwd"].Value = pwd;
            NpgsqlDataReader dr = c.ExecuteReader();
            if (dr.Read())
            {
                //0-name, 1-pwd, 2-coins, 3-collection, 4-elo, 5-win, 6-loss
                return new Player((string)dr[0], (int)dr[4], (int)dr[2], (int)dr[3], (int)dr[5], (int)dr[6]);
            }
            return null;
        }

        public void UpdatePlayer(Player toUpdate) //update literally every value except name & pwd
        {
            IDbCommand command = Connect();
            command.CommandText = @"UPDATE players
                SET coins= @coins,collection=@collection,elo = @elo,win=@win,loss=@loss
                WHERE name = @name";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.Add("name", NpgsqlDbType.Varchar, 32);
            c.Parameters.Add("coins", NpgsqlDbType.Integer);
            c.Parameters.Add("collection", NpgsqlDbType.Integer);
            c.Parameters.Add("elo", NpgsqlDbType.Integer);
            c.Parameters.Add("win", NpgsqlDbType.Integer);
            c.Parameters.Add("loss", NpgsqlDbType.Integer);
            c.Prepare();
            c.Parameters["name"].Value = toUpdate.Name;
            c.Parameters["coins"].Value = toUpdate.Coins;
            c.Parameters["collection"].Value = toUpdate.GetCollectionInt();
            c.Parameters["elo"].Value = toUpdate.Elo;
            c.Parameters["win"].Value = toUpdate.Wins;
            c.Parameters["loss"].Value = toUpdate.Losses;
            command.ExecuteNonQuery();

        }
        //update player deck. add 4 ints for deck in pgsql as well & be like default sets 0 to -1 and rest to 0

        public IDbCommand Connect()
        {
            IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=123;Database=MTCG");
            connection.Open();
            IDbCommand command = connection.CreateCommand();

            return command;
            
        }

        
    }
}
