using System;
using System.Collections.Generic;
using System.Text;

namespace Juventus.SOM.Sudoku.Models
{
    class SudokuApiResponse
    {
        public bool Response { get; set; }
        public string Size { get; set; }
        public IEnumerable<Field> Squares { get; set; }
    }
}
