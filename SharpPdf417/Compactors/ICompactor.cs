using System;
using System.Collections.Generic;

namespace SharpPdf417.Compactors
{
    internal interface ICompactor
    {
        List<int> GenerateCodewords(Sequence sequence);
    }
}
