using System.Collections.Generic;

namespace SharpPdf417.Matchers
{
    internal interface ISequenceMatcher
    {
        List<SequencePosition> GetSequencePositions(string input);
    }
}
