using System;
using System.Collections.Generic;

namespace SharpPdf417.Compactors
{
    internal class NumberModeCompactor : AbstractCompactor 
    {
        public override List<int> GenerateCodewords(Sequence sequence) 
        {
            List<int> codewords = new List<int>();
            string rest;
            string code = sequence.Code;
            while (code.Length > 0) 
            {
                if (code.Length > 44) 
                {
                    rest = code.Substring(44);
                    code = code.Substring(0, 44);
                } 
                else 
                {
                    rest = "";
                }
                long t = long.Parse("1" + code);
                do 
                {
                    long d = t % 900;
                    t = t / 900;
                    codewords.Insert(0, (int)d);
                } 
                while (t != 0);
                code = rest;
            }
            return AddModeToCodeWords(codewords, sequence.Mode);
        }
    }
}
