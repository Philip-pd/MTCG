using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.SystemLogicClasses
{
    class Trade
    {
        public int ID { get; }
        public string Owner { get; }
        public int CardOffered { get; }
        public int CardRequested { get;}
        public int MoneyRequested { get;}

        public Trade(int id,string owner,int offered,int cardr,int moneyr)
        {
            this.ID = id;
            this.Owner = owner;
            this.CardOffered = offered;
            this.CardRequested = cardr;
            this.MoneyRequested = moneyr;
        }

        
    }
}
