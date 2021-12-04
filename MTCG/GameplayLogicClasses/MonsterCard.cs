using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.GameplayLogicClasses
{
    abstract class MonsterCard : Card
    {
        public string Name { get; }

        public override string GetName()
        {
            return ($"{ this.Element}{ this.Name}");
        }
        public abstract bool SpecialRulesCheck(Card Enemy);

    }
}
