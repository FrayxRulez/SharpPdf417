using System;

namespace SharpPdf417.Arrays
{
    internal static class ArrayUtil
    {
        /**
         * Finds a needle in an (unsorted) haystack.
         * @param haystack
         * @param needle
         * @return the key for the needle, or -1 if the needle doesn't exist in the haystack
         */
        public static int Search(int[] haystack, int needle)
        {
            if (haystack == null)
            {
                throw new ArgumentNullException("Haystack is null and cannot be searched");
            }

            for (int i = 0; i < haystack.Length; i++)
            {
                if (haystack[i] == needle)
                {
                    return i;
                }
            }
            return -1;
        }


        public static int[] Reverse(int[] input)
        {
            int[] reversed = new int[input.Length];
            for (int i = input.Length - 1, j = 0; i >= 0; i--, j++)
            {
                reversed[j] = input[i];
            }
            return reversed;
        }

        public static int[] Fill(int size, int i)
        {
            int[] array = new int[size];
            
            //Arrays.fill(array, i);
            
            for (var j = 0; j < i; j++)
            {
                array[j] = i;
            }

            return array;
        }

        public static int[] ToIntegerArray(string row)
        {
            char[] chars = row.ToCharArray();
            int[] ints = new int[chars.Length];
            for (int i = 0; i < chars.Length; i++)
            {
                ints[i] = int.Parse("" + chars[i]);
            }
            return ints;
        }
    }
}
