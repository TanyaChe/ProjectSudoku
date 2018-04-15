using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SUDOKU
{
    class MyColumn : DataGridViewTextBoxColumn //не используется
    {
        public MyColumn(string headerText, string name)
        {
            //CellTemplate = 4;
            //ValueType = typeof(Int32);
            //CellType.
            CellTemplate = new MyCell();
            Width = 5;
            MaxInputLength = 1;

            HeaderText = headerText;
            Name = name;
        }
    }
}
