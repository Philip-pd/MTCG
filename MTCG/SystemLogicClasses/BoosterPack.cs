using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.SystemLogicClasses
{
    public class BoosterPack
    {
        public int[] GetPack(int selector)
        {
            switch (selector)
            {
                case 0:
                    return new int[] { 0, 1, 2, 3, 4 };
                case 1:
                    return new int[] { 5, 6, 7, 8, 9 };
                case 2:
                    return new int[] { 10, 11, 12, 13, 14 };
                case 3:
                    return new int[] { 15, 16, 17, 18, 19 };
                case 4:
                    return new int[] { 20, 21, 22, 23, 24 };
                case 5:
                    return new int[] { 25, 26, 27, 28, 29 };
                case 6:
                    return new int[] { 5, 10, 15, 20, 25 };
                case 7:
                    return new int[] { 6, 11, 16, 21, 26 };
                case 8:
                    return new int[] { 7, 12, 17, 22, 27 };
                case 9:
                    return new int[] { 8, 13, 18, 23, 28 };
                case 10:
                    return new int[] { 9, 14, 19, 24, 29 };
                default:
                    return null;
            }
        }
    }
}
