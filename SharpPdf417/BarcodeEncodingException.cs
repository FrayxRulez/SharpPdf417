using System;

namespace SharpPdf417
{
    public class BarcodeEncodingException : Exception 
    {
        public BarcodeEncodingException(string message) 
            : base(message)
        {
        }
    }
}
