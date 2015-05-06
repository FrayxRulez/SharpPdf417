using System;
using System.Collections.Generic;

namespace SharpPdf417.Compactors
{
    internal abstract class AbstractCompactor : ICompactor 
    {
        public List<int> AddModeToCodeWords(List<int> codewords, SequenceMode sequenceMode) 
        {
            codewords.Insert(0, (int)sequenceMode);
            return codewords;
        }

        public abstract List<int> GenerateCodewords(Sequence sequence);
    }
}
