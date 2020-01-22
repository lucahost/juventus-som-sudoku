using Juventus.SOM.Sudoku.Services;
using Juventus.SOM.Sudoku.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Prism.Events;
using Prism.Navigation;
using Juventus.SOM.Sudoku.Events;
using System;

namespace Juventus.SOM.Sudoku.Test
{
    public class TestBase
    {
        protected Mock<ISudokuService> sudokuServiceMock = new Mock<ISudokuService>();
        protected Mock<IEventAggregator> eventAggregatorMock = new Mock<IEventAggregator>();
        protected Mock<INavigationService> navigationServiceMock = new Mock<INavigationService>();

        protected Mock<SudokuFieldLoadedEvent> sudokuFieldLoadedEventMock = new Mock<SudokuFieldLoadedEvent>();

        protected readonly List<Field> fields;

        public TestBase()
        {
            fields = new List<Field>();

            for (int i = 0; i < 81; i++)
            {
                var field = new Field(i, i, i);
                fields.Add(field);
            }
        }

        protected void TestSetup()
        {
            SetupEvents();
            AggregatorSetup();
            SudokuServiceSetup();
        }

        private void SetupEvents()
        {
            sudokuFieldLoadedEventMock = new Mock<SudokuFieldLoadedEvent>();
        }

        private void AggregatorSetup()
        {
            eventAggregatorMock.Setup(x => x.GetEvent<SudokuFieldLoadedEvent>()).Returns(sudokuFieldLoadedEventMock.Object);
        }


        private void SudokuServiceSetup()
        {
            sudokuServiceMock.Setup(x => x.InitFields()).Returns(() => new List<Field>());
            sudokuServiceMock.Setup(x => x.GetSudokuGameFromWebApi(GameDifficulty.Easy)).Returns(() => Task.FromResult(fields));
            sudokuServiceMock.Setup(x => x.ReplacePreDefinedFields(It.IsAny<List<Field>>(), It.IsAny<List<Field>>())).Returns<List<Field>, List<Field>>((baseList, preDefined) => preDefined);
        }
    }
}
