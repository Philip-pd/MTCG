﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.GameplayLogicClasses
{
    enum cardname
    {
        Goblin,
        Dragon,
        Wizzard,
        Orc,
        Knight,
        Kraken,
        Elf,
        Troll,
        Duck
    }
    public class CardFactory
    {
        public int ElementTypes { get; }
        public int CardTypes { get; }
        public CardFactory()
        {
            this.ElementTypes = element.GetNames(typeof(element)).Length;
            this.CardTypes = cardname.GetNames(typeof(cardname)).Length; //might be unneccessary
        }
        public Card GenerateCard(int id)
        {
            Card Gen = null;
            element _Element = GetElement(id); // Element Offset
            int _Type =( id - (int)_Element)/ ElementTypes;
            switch (_Type) //remove Element Offset from ID  //Modulo Element Types for expandability
            {
                case 0:
                    Gen = new Goblin(_Element, 20, Enum.GetName(typeof(cardname), _Type));
                    break;
                case 1:
                    Gen = new Dragon(_Element, 30, Enum.GetName(typeof(cardname), _Type)); 
                    break;
                case 2:
                    Gen = new Wizzard(_Element, 11, Enum.GetName(typeof(cardname), _Type));
                    break;
                case 3:
                    Gen = new Orc(_Element, 25, Enum.GetName(typeof(cardname), _Type));
                    break;
                case 4:
                    Gen = new Knight(_Element, 25, Enum.GetName(typeof(cardname), _Type));
                    break;
                case 5:
                    Gen = new Kraken(_Element, 55, Enum.GetName(typeof(cardname), _Type));
                    break;
                case 6:
                    Gen = new Elf(_Element, 24, Enum.GetName(typeof(cardname), _Type));
                    break;
                case 7:
                    Gen = new Troll(_Element, 30, Enum.GetName(typeof(cardname), _Type));
                    break;
                case 8:
                    Gen = new Duck(_Element, 5, Enum.GetName(typeof(cardname), _Type));
                    break;
                case 9:
                    Gen = new SpellCard(_Element, 20);
                    break;

                default:
                    break;
            }


            return Gen; //try catch if it is null outside
        }
        element GetElement(int id)
        {
            return (element)(id % this.ElementTypes); //check if enum really doesn't loop
        }
    }
}