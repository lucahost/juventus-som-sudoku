using System;
using System.Collections.Generic;
using System.Text;

namespace Juventus.SOM.Sudoku.Models
{
    public class Field
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Value { get; set; }

        public Field(int x, int y, int value = -1)
        {
            X = x;
            Y = y;
            Value = value;
        }
    }
}
