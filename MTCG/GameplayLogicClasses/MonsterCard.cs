using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.GameplayLogicClasses
{
    abstract class MonsterCard : Card
    {
        public string Name { get; }

        public MonsterCard(element _Element, int _Damage,string _Name) : base(_Element, _Damage)
        {
            this.Name = _Name;
        }
        public override string GetName()
        {
            return ($"{ this.Element}{ this.Name}");
        }
        public abstract bool SpecialRulesCheck(MonsterCard Enemy);
    }
}
