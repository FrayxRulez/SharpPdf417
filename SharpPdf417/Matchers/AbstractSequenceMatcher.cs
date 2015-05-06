using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SharpPdf417.Matchers
{
    internal abstract class AbstractSequenceMatcher : ISequenceMatcher 
    {
        public List<SequencePosition> GetSequencePositions(string input) 
        {
            List<SequencePosition> sequencePositions = new List<SequencePosition>();
            //Matcher sequencesMatcher = getPattern().matcher(input);
            Match sequencesMatcher = GetPattern().Match(input);

            //if (sequencesMatcher.matches())
            if (GetPattern().IsMatch(input))
            {
                for (int i = 1; i <= sequencesMatcher.Groups.Count; i++) 
                {
                    string group = sequencesMatcher.Groups[i].Value;
                    int offset = input.IndexOf(group);
                    sequencePositions.Add(new SequencePosition(group, offset));
                }
            }
            sequencePositions.Add(new SequencePosition("", input.Length));
            return sequencePositions;
        }

        protected abstract Regex GetPattern();
    }
}
