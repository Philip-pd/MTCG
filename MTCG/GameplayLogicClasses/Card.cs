using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.GameplayLogicClasses
{
    public enum element
    {
        Water,
        Fire,
        Normal,
    }
    public abstract class Card
    {
        public element Element { get; }
        public int Damage {get;}

        public Card(element _Element,int _Damage)
        {
            this.Element = _Element;
            this.Damage = _Damage;
        }

        public abstract string GetName();
        public abstract int GetDamage(Card Enemy);

        
        
    }
}
