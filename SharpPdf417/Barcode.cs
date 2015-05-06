using SharpPdf417.Arrays;
using System.Collections.Generic;

namespace SharpPdf417
{
    public class Barcode 
    {
        private int _numRows;
        private int _numCols;
        private int[][] _barcode;

        protected Barcode(int numRows, int numCols, int[][] barcode) 
        {
            _numRows = numRows;
            _numCols = numCols;
            _barcode = barcode;
        }

        public int Columns
        {
            get { return _numCols; }
        }

        public int Rows
        {
            get { return _numRows; }
        }

        public int[][] RawData
        {
            get { return _barcode; }
        }

        internal class Builder 
        {
            private int _numRows;
            private int _numCols;
            private List<int[]> _rows;
            private int _quietZoneHorizonal;
            private int _quietZoneVertical;
            private int _rowHeight;
            private int _dataWidth;
            private int _dataHeight;

            public Builder() 
            {
                _rows = new List<int[]>();
            }

            public Builder SetQuietZoneHorizonal(int quietZoneHorizonal) 
            {
                _quietZoneHorizonal = quietZoneHorizonal;
                return this;
            }

            public Builder SetQuietZoneVertical(int quietZoneVertical) 
            {
                _quietZoneVertical = quietZoneVertical;
                return this;
            }

            public Builder AddRow(int[] rowData) 
            {
                _rows.Add(rowData);
                return this;
            }

            public Builder SetRows(List<int[]> rows) 
            {
                _rows = rows;
                return this;
            }

            public Barcode Build() 
            {
                _numRows = CalculateNumberOfRows();
                _numCols = CalculateNumberOfColumns();

                //int[][] barcodeData = new int[numRows][numCols];
                int[][] barcodeData = new int[_numRows][];
                for (int j = 0; j < barcodeData.Length; j++)
                {
                    barcodeData[j] = new int[_numCols];
                }

                int rowNum = 0;
                AddHorizontalQuietZone(barcodeData, rowNum, _quietZoneHorizonal);

                int i = 0;
                for (rowNum = _quietZoneHorizonal; rowNum < _numRows - _quietZoneHorizonal; rowNum++, i++) 
                {
                    barcodeData[rowNum] = _rows[i];
                }

                AddHorizontalQuietZone(barcodeData, rowNum, _quietZoneHorizonal);

                return new Barcode(_numRows, _numCols, barcodeData);
            }

            private void AddHorizontalQuietZone(int[][] barcodeData, int startRow, int numRows) 
            {
                for (int i = 0; i < numRows; i++) 
                {
                    barcodeData[i + startRow] = ArrayUtil.Fill(_numCols, 0);
                }
            }

            private int CalculateNumberOfColumns() 
            {
                return ((_dataWidth + 2) * 17) + 35 + (2 * _quietZoneHorizonal);
            }

            private int CalculateNumberOfRows() 
            {
                return (_dataHeight * _rowHeight) + (2 * _quietZoneVertical);
            }

            public void SetRowHeight(int rowHeight) 
            {
                _rowHeight = rowHeight;
            }

            public void SetDataWidth(int dataWidth) 
            {
                _dataWidth = dataWidth;
            }

            public void SetDataHeight(int dataHeight) 
            {
                _dataHeight = dataHeight;
            }

        }
    }
}
