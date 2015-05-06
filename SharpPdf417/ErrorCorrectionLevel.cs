using SharpPdf417.Arrays;
using SharpPdf417.Constants;
using System;
using System.Collections.Generic;

namespace SharpPdf417
{
    public class ErrorCorrectionLevel
    {
        private int errorCorrectionLevel;
        private int[] errorCorrectionCoefficients;

        internal ErrorCorrectionLevel(int errorCorrectionLevel, int[] errorCorrectionCoefficients)
        {
            this.errorCorrectionLevel = errorCorrectionLevel;
            this.errorCorrectionCoefficients = errorCorrectionCoefficients;
        }

        internal int[] GetErrorCorrection(List<int> codewords)
        {
            int errorCorrectionSize = (2 << errorCorrectionLevel);
            int errorCorrectionLevelMaxId = (errorCorrectionSize - 1);
            int[] errorCodeWords = ArrayUtil.Fill(errorCorrectionSize, 0);

            for (int i = 0; i < codewords.Count; i++)
            {
                int t1 = (codewords[i] + errorCodeWords[errorCorrectionLevelMaxId] % 929);
                for (int j = errorCorrectionLevelMaxId; j > 0; j--)
                {
                    int t2 = (t1 * errorCorrectionCoefficients[j]) % 929;
                    int t3 = 929 - t2;
                    errorCodeWords[j] = (errorCodeWords[j - 1] + t3) % 929;
                }
                int t21 = (t1 * errorCorrectionCoefficients[0]) % 929;
                int t31 = 929 - t21;
                errorCodeWords[0] = t31 % 929;
            }
            for (int i = 0; i < errorCodeWords.Length; i++)
            {
                if (errorCodeWords[i] != 0)
                {
                    errorCodeWords[i] = 929 - errorCodeWords[i];
                }
            }
            return ArrayUtil.Reverse(errorCodeWords);
        }

        public int Level
        {
            get { return errorCorrectionLevel; }
        }

        public int Size
        {
            get { return (2 << errorCorrectionLevel); }
        }

        internal List<int> AddErrorCorrectionWords(List<int> codewords)
        {
            int[] errorCorrectionWords = GetErrorCorrection(codewords);

            // add error correction
            //for (int errorCorrectionWord : errorCorrectionWords) 
            foreach (int errorCorrectionWord in errorCorrectionWords)
            {
                codewords.Add(errorCorrectionWord);
            }
            return codewords;
        }

        #region static
        // From ErrorCollection.cs, now removed to simplify using.
        public static ErrorCorrectionLevel LevelZero = new ErrorCorrectionLevel(0, RSFactors.RS_FACTORS[0]);
        public static ErrorCorrectionLevel LevelOne = new ErrorCorrectionLevel(1, RSFactors.RS_FACTORS[1]);
        public static ErrorCorrectionLevel LevelTwo = new ErrorCorrectionLevel(2, RSFactors.RS_FACTORS[2]);
        public static ErrorCorrectionLevel LevelThree = new ErrorCorrectionLevel(3, RSFactors.RS_FACTORS[3]);
        public static ErrorCorrectionLevel LevelFour = new ErrorCorrectionLevel(4, RSFactors.RS_FACTORS[4]);
        public static ErrorCorrectionLevel LevelFive = new ErrorCorrectionLevel(5, RSFactors.RS_FACTORS[5]);
        public static ErrorCorrectionLevel LevelSix = new ErrorCorrectionLevel(6, RSFactors.RS_FACTORS[6]);
        public static ErrorCorrectionLevel LevelSeven = new ErrorCorrectionLevel(7, RSFactors.RS_FACTORS[7]);
        public static ErrorCorrectionLevel LevelEight = new ErrorCorrectionLevel(8, RSFactors.RS_FACTORS[8]);

        private static ErrorCorrectionLevel[] errorCorrectionOptions = new ErrorCorrectionLevel[]
        {
            LevelEight,
            LevelSeven,
            LevelSix,
            LevelFive,
            LevelFour,
            LevelThree,
            LevelTwo,
            LevelOne,
            LevelZero
        };

        public static ErrorCorrectionLevel DefaultLevel = LevelTwo;

        public static ErrorCorrectionLevel GetErrorCorrectionLevel(ErrorCorrectionLevel errorCorrection, int size)
        {
            ErrorCorrectionLevel chosenErrorCorrection = errorCorrection;
            int maxErrorCorrectionLevel = 8;
            int maxErrorSize = (928 - size);

            while (maxErrorCorrectionLevel > 0)
            {
                int errorSize = (2 << errorCorrection.Level);
                if (maxErrorSize > errorSize)
                {
                    break;
                }
                --maxErrorCorrectionLevel;
            }

            if ((errorCorrection.Level < 0) || (errorCorrection.Level > 8))
            {
                if (size < 41)
                {
                    chosenErrorCorrection = LevelTwo;
                }
                else if (size < 161)
                {
                    chosenErrorCorrection = LevelThree;
                }
                else if (size < 321)
                {
                    chosenErrorCorrection = LevelFour;
                }
                else if (size < 864)
                {
                    chosenErrorCorrection = LevelFive;
                }
                else
                {
                    chosenErrorCorrection = FindErrorCorrectionLevel(maxErrorCorrectionLevel);
                }
            }

            return chosenErrorCorrection;
        }

        private static ErrorCorrectionLevel FindErrorCorrectionLevel(int level)
        {
            //for (ErrorCorrectionLevel errorCorrectionLevel : errorCorrectionOptions) {
            foreach (ErrorCorrectionLevel errorCorrectionLevel in errorCorrectionOptions)
            {
                if (errorCorrectionLevel.Level == level)
                {
                    return errorCorrectionLevel;
                }
            }
            throw new ArgumentException("Invald level specified");
        }
        #endregion
    }
}
