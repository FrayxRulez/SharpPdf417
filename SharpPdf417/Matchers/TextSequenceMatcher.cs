using System.Text.RegularExpressions;

namespace SharpPdf417.Matchers
{
    internal class TextSequenceMatcher : AbstractSequenceMatcher 
    {
        private static string SEQUENCE_PATTERN = "([\\x09\\x0a\\x0d\\x20-\\x7e]{5,})";
        private static Regex PATTERN = new Regex(SEQUENCE_PATTERN); //, RegexOptions.Compiled);

        protected override Regex GetPattern()
        {
            return PATTERN;
        }
    }
}
