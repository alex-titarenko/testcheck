using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using TAlex.Testcheck.Core;

namespace TAlex.Testcheck.Core.Helpers
{
    public static class Shuffles
    {
        #region Fields

        private static Random _rnd = new Random();

        #endregion

        #region Methods

        public static void Shuffle<T>(IList<T> list)
        {
            Shuffle<T>(list, list.Count);
        }

        public static void Shuffle<T>(IList<T> list, Random rand)
        {
            Shuffle<T>(list, list.Count, rand);
        }

        public static void Shuffle<T>(IList<T> list, int itemCount)
        {
            Shuffle<T>(list, itemCount, _rnd);
        }

        public static void Shuffle<T>(IList<T> list, int itemCount, Random rand)
        {
            if (itemCount < 0)
                return;

            int n = itemCount * itemCount;

            for (int i = 0; i < n; i++)
            {
                int first = rand.Next(itemCount);
                int second = rand.Next(itemCount);

                if (first != second)
                {
                    T temt = list[first];
                    list[first] = list[second];
                    list[second] = temt;
                }
            }
        }

        public static void Shuffle<T>(IList<T> list, ShuffleMode mode)
        {
            Shuffle<T>(list, mode, _rnd);
        }

        public static void Shuffle<T>(IList<T> list, ShuffleMode mode, Random rand)
        {
            switch (mode)
            {
                case ShuffleMode.None:
                    break;

                case ShuffleMode.All:
                    Shuffle<T>(list, rand);
                    break;

                case ShuffleMode.ExceptLast:
                    Shuffle<T>(list, list.Count - 1, rand);
                    break;

                case ShuffleMode.ExceptTwoLast:
                    Shuffle<T>(list, list.Count - 2, rand);
                    break;
            }
        }

        public static int[] GetRandomSequence(int length, ShuffleMode mode)
        {
            return GetRandomSequence(length, mode, _rnd);
        }

        public static int[] GetRandomSequence(int length, ShuffleMode mode, Random rand)
        {
            int[] sequence = new int[length];
            
            for (int i = 0; i < length; i++)
                sequence[i] = i;

            Shuffle<int>(sequence, mode, rand);
            return sequence;
        }

        public static void ReorderBySequence<T>(IList<T> list, int[] sequence)
        {
            IList<T> tempList = new List<T>();
            foreach (int index in sequence)
            {
                tempList.Add(list[index]);
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i] = tempList[i];
            }
        }

        #endregion
    }
}
