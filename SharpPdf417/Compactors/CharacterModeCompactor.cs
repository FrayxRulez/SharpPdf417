using System;
using System.Collections.Generic;

namespace SharpPdf417.Compactors
{
    internal class CharacterModeCompactor : AbstractCompactor 
    {
        public override List<int> GenerateCodewords(Sequence sequence) 
        {
            List<int> codewords = new List<int>();
            codewords.Add((int)sequence.Code[0]);
            return AddModeToCodeWords(codewords, sequence.Mode);
        }
    }
}
