using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.GameplayLogicClasses
{
    class Troll : MonsterCard
    {
        public Troll(element _Element, int _Damage, string _Name) : base(_Element, _Damage, _Name) { }

        public override int GetDamage(Card Enemy)
        {
            if (Enemy.GetType().Equals(typeof(SpellCard)))
            {
                if (((int)this.Element + 1) % 3 == (int)Enemy.Element)
                {
                    return this.Damage * 2;
                }
                else if (((int)this.Element - 1) % 3 == (int)Enemy.Element)
                {
                    return this.Damage / 2;
                }
                else
                { return this.Damage; }
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
            return false;
        }
    }
}
