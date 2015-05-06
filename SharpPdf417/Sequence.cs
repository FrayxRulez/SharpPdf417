using System;

namespace SharpPdf417
{
    public class Sequence 
    {
        private SequenceMode _mode;
        private string _code;

        public Sequence(SequenceMode mode, string code) 
        {
            _mode = mode;
            _code = code;
        }

        public SequenceMode Mode 
        {
            get { return _mode; }
        }

        public string Code
        {
            get { return _code; }
        }
    }
}
