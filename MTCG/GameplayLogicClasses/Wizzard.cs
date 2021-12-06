using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.GameplayLogicClasses
{
    class Wizzard : MonsterCard
    {
        public Wizzard(element _Element, int _Damage, string _Name) : base(_Element, _Damage, _Name) { }

        public override int GetDamage(Card Enemy) //Wizzards fight using spells so Elements also apply here but they only get buffed
        {
            int bonus = 0;
            if (SpecialRulesCheck((MonsterCard)Enemy))
                bonus = 2;

                if (((int)this.Element + 1) % 3 == (int)Enemy.Element)
                {
                    return this.Damage * 2*bonus;
                }
                else
                { return this.Damage * bonus; }
            } 
        public override bool SpecialRulesCheck(MonsterCard Enemy)
        {
            if (Enemy.Name == "Troll" || Enemy.Name == "Goblin" || Enemy.Name == "Orc")
                return true;
            return false;
        }
    }
}
