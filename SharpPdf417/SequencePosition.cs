using System;

namespace SharpPdf417
{
    public class SequencePosition 
    {
        private string _sequence;
        private int _offset;

        public SequencePosition(string sequence, int offset) 
        {
            _sequence = sequence;
            _offset = offset;
        }

        public int Offset
        {
            get { return _offset; }
        }

        public string Sequence
        {
            get { return _sequence; }
        }
    }
}
