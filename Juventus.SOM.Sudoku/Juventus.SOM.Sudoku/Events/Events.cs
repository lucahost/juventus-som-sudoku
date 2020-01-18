using System;
using System.Collections.Generic;
using System.Text;
using Juventus.SOM.Sudoku.Models;
using Prism.Events;

namespace Juventus.SOM.Sudoku.Events
{
    public class SudokuFieldLoadedEvent : PubSubEvent<List<Field>>
    {
    }
}
