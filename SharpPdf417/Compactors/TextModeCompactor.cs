using System;
using System.Collections.Generic;
using SharpPdf417.Arrays;
using SharpPdf417.Constants;

namespace SharpPdf417.Compactors
{
    internal class TextModeCompactor : AbstractCompactor 
    {
        public override List<int> GenerateCodewords(Sequence sequence) 
        {
            List<int> codewords = new List<int>();
            int submode = 0;
            List<int> textArray = new List<int>();
            int codeLength = sequence.Code.Length;

            for (int i = 0; i < codeLength; i++) 
            {
                int charVal = (int)sequence.Code[i];
                int indexOfCharacter = ArrayUtil.Search(TextSubmodes.TEXT_SUBMODES[submode], charVal);
                if (indexOfCharacter != -1) 
                {
                    // same submode
                    textArray.Add(indexOfCharacter);
                } 
                else 
                {
                    // submode changed
                    for (int s = 0; s < 4; s++) 
                    {
                        // search for new submode
                        indexOfCharacter = ArrayUtil.Search(TextSubmodes.TEXT_SUBMODES[s], charVal);
                        if (s != submode && indexOfCharacter != -1) 
                        {
                            // s is new submode
                            if (
                                    (
                                        ((i + 1) == codeLength) ||
                                            (
                                                ((i + 1) < codeLength) &&
                                                    (ArrayUtil.Search(TextSubmodes.TEXT_SUBMODES[submode], (int) sequence.Code[i + 1]) != -1)
                                            ) &&
                                            (
                                                (s == 3) ||
                                                    ((s == 0) && (submode == 1))
                                            )
                                    )
                                    ) {
                                if (s == 3) 
                                {
                                    // punctuate
                                    textArray.Add(29);
                                } 
                                else 
                                {
                                    // lower to alpha
                                    textArray.Add(27);
                                }

                            } 
                            else 
                            {
                                // latch
                                //textArray.addAll(TextLatches.TEXT_LATCH.get(string.format("%d%d", submode, s)));
                                textArray.AddRange(TextLatches.TEXT_LATCH[string.Format("{0}{1}", submode, s)]);
                                submode = s;
                            }

                            textArray.Add(indexOfCharacter);
                            break;
                        }
                    }
                }
            }

            int textArrayLength = textArray.Count;
            if (textArrayLength % 2 != 0) 
            {
                textArray.Add(29);
                textArrayLength++;
            }

            for (int i = 0; i < textArrayLength; i+=2) 
            {
                if (i < textArrayLength - 1) 
                {
                    codewords.Add((30 * textArray[i] + textArray[i + 1]));
                } 
                else 
                {
                    codewords.Add((30 * textArray[i]));
                }
            }

            return AddModeToCodeWords(codewords, sequence.Mode);
        }
    }
}