using System;
using System.Collections.Generic;
using System.Text;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace MTCG.SystemLogicClasses
{
    public class TradeDAOImpl : TradeDAO
    {
        public bool AcceptTrade(Player accepter, int id)
        {
            Trade trade = GetTrade(id);
            if (trade == null)
                return false;
            int cardr = trade.CardRequested;
            int coinsr = trade.MoneyRequested;
            int cardo = trade.CardOffered;
            if (cardr!=-1)
            {
               //cant trade a card you don't have or trade for a card you already own
                if (!accepter.Collection[cardr] || accepter.Collection[cardo]) 
                    return false;
                accepter.Collection[cardr] = false;
                accepter.Collection[cardo] = true;
                PlayerDAO dao = new PlayerDAOImpl();
                dao.UpdatePlayer(accepter);
                UpdateTrader(trade);
                DeleteTrade(id);
                return true;


            } else
            {
               //cant trade with coins you don't have or trade for a card you already own
                if (accepter.Collection[cardo] || accepter.Coins < coinsr) 
                    return false;
                accepter.Coins -= coinsr;
                accepter.Collection[cardo] = true;
                PlayerDAO dao = new PlayerDAOImpl();
                dao.UpdatePlayer(accepter);
                UpdateTrader(trade);
                DeleteTrade(id);
                return true;
            }
        }

        public bool CancelTrade(Player from, int id)
        {
            Trade trade = GetTrade(id);
            if (trade == null)
                return false;
            if (from.Name != trade.Owner)
                return false;
            if(from.Collection[trade.CardOffered] == true) //in case you cancel after aquiring card again
            {
                from.Coins += 1;
            }else
            {
                from.Collection[trade.CardOffered] = true;
            }
            DeleteTrade(id);
            return true;

        }

        public bool CreateTrade(Player from, Trade trade)
        {
            if(!from.Collection[trade.CardOffered]) //lock
                return false;

            IDbCommand command = Connect();
            command.CommandText = @"insert into trades(name,Offered,WantsC,WantsM)
                values(@name,@cardo,@cardr,@coinsr)"; 
            NpgsqlCommand c = command as NpgsqlCommand; 
            c.Parameters.Add("name", NpgsqlDbType.Varchar, 32);
            c.Parameters.Add("cardo", NpgsqlDbType.Integer);
            c.Parameters.Add("cardr", NpgsqlDbType.Integer);
            c.Parameters.Add("coinsr", NpgsqlDbType.Integer);
            c.Prepare();
            c.Parameters["name"].Value = from.Name;
            c.Parameters["cardo"].Value = trade.CardOffered;
            c.Parameters["cardr"].Value = trade.CardRequested;
            c.Parameters["coinsr"].Value = trade.MoneyRequested;
            try //try executing the command
            {

                command.ExecuteNonQuery();
                from.Collection[trade.CardOffered] = false; //lock 
                PlayerDAO dao = new PlayerDAOImpl();
                dao.UpdatePlayer(from); //only take out if success
                return true;
            }
            catch (NpgsqlException)
            {
                return false;
            }
            
        }

        public List<Trade> GetAllTrades()
        {
            List<Trade> trades = new List<Trade>();

            IDbCommand command = Connect();
            command.CommandText = @"SELECT * FROM trades";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Prepare();
            NpgsqlDataReader dr = c.ExecuteReader();
            while (dr.Read())
            {
                //0-TID, 1-name, 2-CardOffered,3-WantsCard,4-WantsCoins 
                Trade toAdd = new Trade((int)dr[0], (string)dr[1], (int)dr[2], (int)dr[3], (int)dr[4]);
                trades.Add(toAdd);
            }

            return trades;
        }

        private void UpdateTrader(Trade trade)
        {
            int card= trade.CardRequested;
            PlayerDAO dao = new PlayerDAOImpl();
            Player toUpdate = dao.GetPlayerInfo(trade.Owner);
            if(card!=-1)
            {
                if (toUpdate.Collection[card] == true)
                {
                    toUpdate.Coins += 1;
                }else
                {
                    toUpdate.Collection[card] = true;
                }
            } else
            {
                toUpdate.Coins += trade.MoneyRequested;
            }
            dao.UpdatePlayer(toUpdate);

        }
        private void DeleteTrade(int id)
        {
            IDbCommand command = Connect();
            command.CommandText = @"DELETE FROM trades
               WHERE tid=@id";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.Add("id", NpgsqlDbType.Integer);
            c.Prepare();
            c.Parameters["id"].Value = id;
            command.ExecuteNonQuery();
        }
        public Trade GetTrade(int id)
        {
            IDbCommand command = Connect();
            command.CommandText = @"SELECT * FROM trades
               WHERE tid=@id";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.Add("id", NpgsqlDbType.Integer);
            c.Prepare();
            c.Parameters["id"].Value = id;
            NpgsqlDataReader dr = c.ExecuteReader();
            if (dr.Read())
            {
                //0-TID, 1-name, 2-Offered,3-WantsC,4-WantsM
                return new Trade((int)dr[0],(string)dr[1], (int)dr[2], (int)dr[3], (int)dr[4]);
            }
            return null;
        }
        private IDbCommand Connect()
        {
            IDbConnection connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=123;Database=MTCG");
            connection.Open();
            IDbCommand command = connection.CreateCommand();

            return command;

        }
    }
}
