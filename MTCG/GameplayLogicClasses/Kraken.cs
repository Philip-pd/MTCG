using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.GameplayLogicClasses
{
    class Kraken:MonsterCard
    {
        public Kraken(element _Element, int _Damage, string _Name) : base(_Element, _Damage, _Name) { }

        public override int GetDamage(Card Enemy)
        {
            if (Enemy.GetType().Equals(typeof(SpellCard)))
            {
                return 999;
            }
            else if (SpecialRulesCheck((MonsterCard)Enemy))
            {
                return 0;
            }
            else
            {
                return this.Damage;
            }

        }

        public override bool SpecialRulesCheck(MonsterCard Enemy)
        {
            if (Enemy.Element==element.Fire) //Will Grill the Kraken
                return true;
            return false;
        }
    }
}
}
