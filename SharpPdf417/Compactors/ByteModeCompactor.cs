using System;
using System.Collections.Generic;

namespace SharpPdf417.Compactors
{
    internal class ByteModeCompactor : AbstractCompactor 
    {
        public override List<int> GenerateCodewords(Sequence sequence) 
        {
            List<int> codewords = new List<int>();

            string rest;
            int subLength;
            string code = sequence.Code;

            while (code.Length > 0) 
            {
                if (code.Length > 6) 
                {
                    rest = code.Substring(6);
                    code = code.Substring(0, 6);
                    subLength = 6;
                } 
                else 
                {
                    rest = "";
                    subLength = code.Length;
                }

                if (subLength == 6) 
                {
                    long t = 0L;
                    t += 1099511627776L * (int)code[0];
                    t += 4294967296L * (int)code[1];
                    t += 16777216L * (int)code[2];
                    t += 65536L * (int)code[3];
                    t += 256L * (int)code[4];
                    t += (int)code[5];

                    do 
                    {
                        long d = t % 900;
                        t = t / 900;
                        codewords.Insert(0, (int)d);
                    } 
                    while (t != 0);
                } 
                else 
                {
                    for (int i = 0; i < subLength; i++) 
                    {
                        codewords.Add((int)code[i]);
                    }
                }
                code = rest;
            }

            return AddModeToCodeWords(codewords, sequence.Mode);
        }
    }
}
