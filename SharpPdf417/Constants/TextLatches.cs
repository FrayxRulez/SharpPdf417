using System;
using System.Collections.Generic;

namespace SharpPdf417.Constants
{
    internal static class TextLatches
    {
        public static Dictionary<string, int[]> TEXT_LATCH = new Dictionary<string, int[]>();

        static TextLatches()
        {
            TextLatches.TEXT_LATCH.Add("01", new int[] { 27 });
            TextLatches.TEXT_LATCH.Add("02", new int[] { 28 });
            TextLatches.TEXT_LATCH.Add("03", new int[] { 28, 25 });
            TextLatches.TEXT_LATCH.Add("10", new int[] { 28, 28 });
            TextLatches.TEXT_LATCH.Add("12", new int[] { 28 });
            TextLatches.TEXT_LATCH.Add("13", new int[] { 28, 25 });
            TextLatches.TEXT_LATCH.Add("20", new int[] { 28 });
            TextLatches.TEXT_LATCH.Add("21", new int[] { 27 });
            TextLatches.TEXT_LATCH.Add("23", new int[] { 25 });
            TextLatches.TEXT_LATCH.Add("30", new int[] { 29 });
            TextLatches.TEXT_LATCH.Add("31", new int[] { 29, 27 });
            TextLatches.TEXT_LATCH.Add("32", new int[] { 29, 28 });
        }
    }
}
