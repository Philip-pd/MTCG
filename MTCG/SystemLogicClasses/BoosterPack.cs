using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MTCG.SystemLogicClasses
{
    public class BoosterPack
    {
        
        public int[] GetPack(int selector)
        {
            IDbCommand command = Connect();
            command.CommandText = @"SELECT * FROM boosterpacks
               WHERE id=@id";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.Add("id", NpgsqlDbType.Integer);
            c.Prepare();
            c.Parameters["id"].Value = selector;
            NpgsqlDataReader dr = c.ExecuteReader();
            if (dr.Read())
            {
                return (int[])dr.GetValue(1);
            }
            return null;
        }

        public bool EnterPacktoDB(int[] cards)
        {
            if (cards.Length != 5)
                return false;
            IDbCommand command = Connect();
            command.CommandText = @"INSERT INTO boosterpacks(cards)
               values(@cards)";
            NpgsqlCommand c = command as NpgsqlCommand;
            c.Parameters.Add("cards", NpgsqlDbType.Array | NpgsqlDbType.Integer);
            c.Prepare();
            c.Parameters["cards"].Value = cards;
            NpgsqlDataReader dr = c.ExecuteReader();
            return true;
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
