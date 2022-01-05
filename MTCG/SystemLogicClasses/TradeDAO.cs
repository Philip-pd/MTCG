using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.SystemLogicClasses
{
    public interface TradeDAO
    {
        List<Trade> GetAllTrades();
        bool CreateTrade(Player from,Trade trade);
        bool CancelTrade(Player from, int id);
        bool AcceptTrade(Player accepter, int id);
        Trade GetTrade(int id);
    }
}
