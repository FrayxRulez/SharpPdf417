using System.Text.RegularExpressions;

namespace SharpPdf417.Matchers
{
    internal class NumberSequenceMatcher : AbstractSequenceMatcher 
    {
        private static string SEQUENCE_PATTERN = "([0-9]{13,44})";
        private static Regex PATTERN = new Regex(SEQUENCE_PATTERN); //, RegexOptions.Compiled);

        protected override Regex GetPattern() 
        {
            return PATTERN;
        }
    }
}
