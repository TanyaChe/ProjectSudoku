using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SUDOKU
{
    class MyGrid : DataGridView
    {
        public MyGrid()
        {
            RowCount = 3;
            ColumnCount = 3;
            //Columns.Add("One", "Two");
            //Columns.Add(new MyColumn());
            //Columns.Add(new MyColumn());
            //AllowDrop = false;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToResizeColumns = false;
            AllowUserToOrderColumns = false;
            AllowUserToResizeRows = false;
            Size = new Size(66, 63);

           /* MyColumn c1 = new MyColumn("Column1", "Column1"),
                c2 = new MyColumn("Column2", "Column2"),
                c3 = new MyColumn("Column3", "Column3");*/

            DataGridViewTextBoxColumn c1 = new DataGridViewTextBoxColumn(), 
                c2 = new DataGridViewTextBoxColumn(), 
                c3 = new DataGridViewTextBoxColumn();
            
            // Column1
            c1.HeaderText = "Column1";
            c1.MaxInputLength = 1;
            c1.Name = "Column1";
            c1.Width = 5;
            c1.CellTemplate = new MyCell();
            
            // Column2
            c2.HeaderText = "Column2";
            c2.MaxInputLength = 1;
            c2.Name = "Column2";
            c2.Width = 5;
            c2.CellTemplate = new MyCell();
            
            // Column3
            c3.HeaderText = "Column3";
            c3.MaxInputLength = 1;
            c3.Name = "Column3";
            c3.Width = 5;
            c3.CellTemplate = new MyCell();
            

            Columns.AddRange(new DataGridViewTextBoxColumn[] {c1,c2,c3});
        }
    }
}
