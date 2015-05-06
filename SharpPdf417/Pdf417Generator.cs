using SharpPdf417.Compactors;
using SharpPdf417.Constants;
using SharpPdf417.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpPdf417
{
    public class Pdf417Generator 
    {
        public static int DEFAULT_ASPECT_RATIO = 2;

        private static int ROW_HEIGHT = 4;
        public static int DEFAULT_QUIET_H = 2;
        public static int DEFAULT_QUIET_V = 2;
        private static int MAX_CODEWORDS_DATA = 925;

        private ErrorCorrectionLevel _errorCorrection;
        private float _aspectRatio;
        private int _quietV;
        private int _quietH;
        private int _codewordIndex;
        private string _input;

        public Pdf417Generator(string input, ErrorCorrectionLevel errorCorrection, float aspectRatio, int quietV, int quietH) 
        {
            _errorCorrection = errorCorrection;
            _aspectRatio = aspectRatio;
            _quietV = quietV;
            _quietH = quietH;
            _input = input;
        }

        public Pdf417Generator(string input) 
            : this(input, ErrorCorrectionLevel.DefaultLevel, DEFAULT_ASPECT_RATIO, DEFAULT_QUIET_V, DEFAULT_QUIET_H)
        {
        }

        //BarcodeEncodingException
        public Barcode Encode()
        {
            List<Sequence> sequences = GetInputSequences(_input);
            List<int> codewords = GenerateCodewords(sequences);

            ErrorCorrectionLevel errorCorrectionLevel = ErrorCorrectionLevel.GetErrorCorrectionLevel(_errorCorrection, codewords.Count);
            int errorSize = errorCorrectionLevel.Size;

            int nce = (codewords.Count + errorSize + 1);

            // calculate number of columns
            int dataWidth = calculateDataWidth(_aspectRatio, ROW_HEIGHT, nce);
            int dataHeight = CalculateDataHeight(nce, dataWidth);
            int size = dataWidth * dataHeight;

            dataWidth = AdjustDataWidth(dataWidth, _aspectRatio, size);
            dataHeight = AdjustDataHeight(dataHeight, _aspectRatio, size);

            if (size > 928) 
            {
                size = 928;
            }

            // add padding it out
            int padding = (size - nce);
            if (padding > 0) 
            {
                if ((size - dataHeight) == nce) 
                {
                    --dataHeight;
                    size -= dataHeight;
                } 
                else 
                {
                    for (int i = 0; i < padding; i++) 
                    {
                        codewords.Add(900);
                    }
                }
            }

            // add symbol length detection
            int symbolLengthDetection = size - errorSize;
            codewords.Insert(0, symbolLengthDetection);

            codewords = errorCorrectionLevel.AddErrorCorrectionWords(codewords);

            Barcode.Builder barcodeBuilder = new Barcode.Builder();
            barcodeBuilder.SetDataWidth(dataWidth);
            barcodeBuilder.SetDataHeight(dataHeight);
            barcodeBuilder.SetQuietZoneHorizonal(_quietH);
            barcodeBuilder.SetQuietZoneVertical(_quietV);
            barcodeBuilder.SetRowHeight(ROW_HEIGHT);

            int barCodeRow = 0;
            _codewordIndex = 0;
            int clusterIndex = 0;

            for (int rowIndex = 0; rowIndex < dataHeight; rowIndex++) 
            {
                int[] rowData = BuildRow(rowIndex, clusterIndex, dataHeight, dataWidth, errorCorrectionLevel, codewords);
                int currentRow = barCodeRow;

                for (; barCodeRow < ROW_HEIGHT + currentRow; barCodeRow++) 
                {
                    barcodeBuilder.AddRow(rowData);
                }

                ++clusterIndex;
                if (clusterIndex > 2) 
                {
                    clusterIndex = 0;
                }
            }

            return barcodeBuilder.Build();
        }

        private int[] BuildRow(int rowIndex, int clusterIndex, int dataHeight, int dataWidth, ErrorCorrectionLevel errorCorrectionLevel, List<int> codewords) 
        {
            BarcodeRowBuilder rowBuilder = new BarcodeRowBuilder();
            rowBuilder.SetQuietZoneHorizontal(_quietH);

            int l;
            switch (clusterIndex) 
            {
                case 0:
                    l = ((30 * ((int) Math.Floor((double)(rowIndex / 3)))) + ((int) Math.Floor((double)((dataHeight - 1) / 3))));
                    break;
                case 1:
                    l = ((30 * ((int) Math.Floor((double)(rowIndex / 3)))) + (errorCorrectionLevel.Level * 3) + ((dataHeight - 1) % 3));
                    break;
                default:
                    l = ((30 * ((int) Math.Floor((double)(rowIndex / 3)))) + (dataWidth - 1));
                    break;
            }

            rowBuilder.AddLeftIndicator(Clusters.CLUSTERS[clusterIndex][l]);

            // for each column
            for (int colIndex = 0; colIndex < dataWidth; colIndex++) 
            {
                rowBuilder.AddData(Clusters.CLUSTERS[clusterIndex][codewords[_codewordIndex]]);
                ++_codewordIndex;
            }

            switch (clusterIndex) 
            {
                case 0:
                    l = ((30 * ((int) Math.Floor((double)(rowIndex / 3)))) + (dataWidth - 1));
                    break;
                case 1:
                    l = ((30 * ((int) Math.Floor((double)(rowIndex / 3)))) + ((int) Math.Floor((double)((dataHeight - 1) / 3))));
                    break;
                default:
                    l = ((30 * ((int) Math.Floor((double)(rowIndex / 3)))) + (errorCorrectionLevel.Level * 3) + ((dataHeight - 1) % 3));
                    break;
            }

            rowBuilder.AddRightIndicator(Clusters.CLUSTERS[clusterIndex][l]);

            return rowBuilder.Build();
        }

        private int AdjustDataWidth(int dataWidth, float aspectRatio, int size) 
        {
            if (size > 928) 
            {
                if (Math.Abs(aspectRatio - (17 * 29 / 32)) < Math.Abs(aspectRatio - (17 * 16 / 58))) 
                {
                    dataWidth = 29;
                }
                else 
                {
                    dataWidth = 16;
                }
            }
            return dataWidth;
        }

        private int AdjustDataHeight(int dataHeight, float aspectRatio, int size) 
        {
            if (size > 928) 
            {
                if (Math.Abs(aspectRatio - (17 * 29 / 32)) < Math.Abs(aspectRatio - (17 * 16 / 58))) 
                {
                    dataHeight = 32;
                } 
                else 
                {
                    dataHeight = 58;
                }
            }
            return dataHeight;
        }

        private int CalculateDataHeight(int nce, int numberOfColumns) 
        {
            int dataHeight = (int) Math.Ceiling((double)(nce / numberOfColumns));
            if (dataHeight < 3) 
            {
                dataHeight = 3;
            } 
            else if (dataHeight > 90) 
            {
                dataHeight = 3;
            }
            return dataHeight;
        }

        private int calculateDataWidth(float aspectRatio, int rowHeight, int nce) 
        {
            int dataWidth = (int) Math.Round((Math.Sqrt(4761 + (68 * aspectRatio * rowHeight * nce)) - 69) / 34);
            // adjust columns
            if (dataWidth < 1) 
            {
                dataWidth = 1;
            }
            else if (dataWidth > 30) 
            {
                dataWidth = 30;
            }
            return dataWidth;
        }

        private List<int> RemoveFirstCodeWord(List<int> codewords) 
        {
            //return codewords.subList(1, codewords.size());
            return codewords.Skip(1).ToList();
        }

        private bool IsTextMode(List<int> codewords) 
        {
            return codewords[0] == (int)SequenceMode.TextMode;
        }

        public List<Sequence> GetInputSequences(string input) 
        {
            List<Sequence> sequences = new List<Sequence>();

            List<SequencePosition> numSequencePositions = new NumberSequenceMatcher().GetSequencePositions(input);


            int offset = 0;
            for (int i = 0; i < numSequencePositions.Count; i++) 
            {
                SequencePosition seq = numSequencePositions[i];
                int seqlen = seq.Sequence.Length;
                if (seq.Offset > 0) 
                {
                    string prevSequence = input.Substring(offset, seq.Offset);
                    List<SequencePosition> textSequencePositions = new TextSequenceMatcher().GetSequencePositions(prevSequence);

                    int textOffset = 0;
                    for (int j = 0; j < textSequencePositions.Count; j++) 
                    {
                        SequencePosition textSequencePosition = textSequencePositions[j];
                        int textSequenceLen = textSequencePosition.Sequence.Length;
                        if (textSequencePosition.Offset > 0) 
                        {
                            string prevTextSequence = prevSequence.Substring(textOffset, textSequencePosition.Offset);
                            if (prevTextSequence.Length > 0)
                            {
                                if ((prevTextSequence.Length == 1) && (sequences.Count > 0) && (sequences[sequences.Count - 1].Mode == SequenceMode.TextMode)) 
                                {
                                    sequences.Add(new Sequence(SequenceMode.ByteModeThree, prevTextSequence));
                                } 
                                else if ((prevTextSequence.Length % 6) == 0) 
                                {
                                    sequences.Add(new Sequence(SequenceMode.ByteModeTwo, prevTextSequence));
                                } 
                                else 
                                {
                                    sequences.Add(new Sequence(SequenceMode.ByteModeOne, prevTextSequence));
                                }
                            }
                        }
                        if (textSequenceLen > 0) 
                        {
                            sequences.Add(new Sequence(SequenceMode.TextMode, textSequencePosition.Sequence));
                        }
                        textOffset = textSequencePosition.Offset + textSequenceLen;
                    }
                }
                if (seqlen > 0) 
                {
                    sequences.Add(new Sequence(SequenceMode.NumberMode, seq.Sequence));
                }
                offset = seq.Offset + seqlen;
            }
            return sequences;
        }

        private List<int> GenerateCodewords(List<Sequence> sequences) //throws BarcodeEncodingException 
        {
            List<int> codewords = new List<int>();
            //for (Sequence sequence : sequences) {
            foreach (Sequence sequence in sequences)
            {
                ICompactor compactor = CompactorFactory.GetCompactor(sequence.Mode);
                List<int> cw = compactor.GenerateCodewords(sequence);
                codewords.AddRange(cw);
            }

            if (IsTextMode(codewords)) 
            {
                codewords = RemoveFirstCodeWord(codewords);
            }

            // too much data to encode
            if (codewords.Count > MAX_CODEWORDS_DATA) 
            {
                throw new BarcodeEncodingException("Too many codewords generated for data. Cannot create barcode.");
            }

            return codewords;
        }
    }
}
