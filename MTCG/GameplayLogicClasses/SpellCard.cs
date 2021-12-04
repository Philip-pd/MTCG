using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.GameplayLogicClasses
{
    class SpellCard : Card
    {
        public override int GetDamage(Card Enemy)
        {
          if(((int)this.Element+1)%3==(int)Enemy.Element)
            {
                return this.Damage*2;
            }
          else if (((int)this.Element - 1) % 3 == (int)Enemy.Element)
            {
                return this.Damage / 2;
            }
          else
            { return this.Damage; }

        }
        public override string GetName()
        {
            return ($"{ this.Element}Spell");
        }
    }
}
