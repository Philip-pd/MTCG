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


        public abstract string GetName();
        public virtual int GetDamage(Card Enemy)
        {
            return this.Damage;
        }
        
        
    }
}
