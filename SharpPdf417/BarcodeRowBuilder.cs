using SharpPdf417.Arrays;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPdf417
{
    internal class BarcodeRowBuilder 
    {
        private static string START_PATTERN = "11111111010101000";
        private static string STOP_PATTERN = "111111101000101001";

        private int quietZoneHorizontal;
        private int leftIndicator;
        private int rightIndicator;
        private List<int> dataList = new List<int>();

        public BarcodeRowBuilder SetQuietZoneHorizontal(int quietZoneHorizontal) 
        {
            this.quietZoneHorizontal = quietZoneHorizontal;
            return this;
        }

        public BarcodeRowBuilder AddLeftIndicator(int leftIndicator) 
        {
            this.leftIndicator = leftIndicator;
            return this;
        }

        public BarcodeRowBuilder AddRightIndicator(int rightIndicator) 
        {
            this.rightIndicator = rightIndicator;
            return this;
        }

        public BarcodeRowBuilder AddData(int data) 
        {
            dataList.Add(data);
            return this;
        }

        public int[] Build() 
        {
            // add horizontal quiet zones
            string pstart = FillString("0", quietZoneHorizontal) + START_PATTERN;
            string pstop = STOP_PATTERN + "" + FillString("0", quietZoneHorizontal);

            StringBuilder rowBuffer = new StringBuilder();
            rowBuffer.Append(pstart);
            rowBuffer.Append(ToBinaryString(leftIndicator));
            //for (Integer dataItem : dataList) {
            //    rowBuffer.append(toBinarystring(dataItem));
            //}
            foreach (int dataItem in dataList)
            {
                rowBuffer.Append(ToBinaryString(dataItem));
            }
            rowBuffer.Append(ToBinaryString(rightIndicator));
            rowBuffer.Append(pstop);
            return ArrayUtil.ToIntegerArray(rowBuffer.ToString());
        }

        private string ToBinaryString(int leftIndicator) 
        {
            //return string.format("%7s", Integer.toBinarystring(leftIndicator)).replaceAll(" ", "1");

            return AddPadding(Convert.ToString(leftIndicator, 2), 7).Replace(" ", "1");
        }

        private string FillString(string s, int size) 
        {
            //return string.format("%" + size + "s", " ").replaceAll(" ", s);
            if (size <= 0)
            {
                return string.Empty;
            }

            return AddPadding(" ", size).Replace(" ", s);
        }

        private string AddPadding(string s, int size)
        {
            string temp = s.ToString();
            while (temp.Length < size)
            {
                temp = " " + temp;
            }

            return temp;
        }
    }
}
