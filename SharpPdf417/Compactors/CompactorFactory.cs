using System;

namespace SharpPdf417.Compactors
{
    internal static class CompactorFactory
    {
        public static ICompactor GetCompactor(SequenceMode sequenceMode)
        {
            switch (sequenceMode)
            {
                case SequenceMode.ByteModeOne:
                case SequenceMode.ByteModeTwo:
                    return new ByteModeCompactor();
                case SequenceMode.ByteModeThree:
                    return new CharacterModeCompactor();
                case SequenceMode.NumberMode:
                    return new NumberModeCompactor();
                default:
                    return new TextModeCompactor();
            }
        }
    }
}
