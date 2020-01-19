using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Juventus.SOM.Sudoku.Core;
using Juventus.SOM.Sudoku.Events;
using Juventus.SOM.Sudoku.Models;
using Juventus.SOM.Sudoku.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;

namespace Juventus.SOM.Sudoku.ViewModels
{
    internal class MainPageViewModel : ViewModelBase
    {
        private string _title;
        private string _outputText;
        private bool _isBusy;
        private readonly ISudokuService _sudokuService;
        private List<Field> _fields;


        public DelegateCommand ValidateCommand { get; private set; }
        public DelegateCommand ReloadCommand { get; private set; }
        public DelegateCommand<Field> TileClickCommand { get; private set; }

        public string Title {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public string OutputText {
            get => _outputText;
            set => SetProperty(ref _outputText, value);
        }

        public bool IsBusy {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }
        public List<Field> Fields {
            get => _fields;
            set => SetProperty(ref _fields, value);
        }

        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, ISudokuService sudokuService)
            : base(navigationService, eventAggregator)
        {
            Title = "Sudoku";
            ReloadCommand = new DelegateCommand(Reload);
            ValidateCommand = new DelegateCommand(Validate);
            TileClickCommand = new DelegateCommand<Field>(TileClick);
            EventAggregator.GetEvent<SudokuFieldLoadedEvent>().Subscribe(FieldsLoaded, ThreadOption.UIThread);
            _sudokuService = sudokuService;
        }

        public override async void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            base.OnNavigatedTo(navigationParameters);
            await InitGame();
        }

        private void FieldsLoaded(List<Field> loadedFields)
        {
            Fields = new List<Field>(loadedFields);
        }

        private async Task InitGame()
        {
            if (_sudokuService != null)
            {
                IsBusy = true;
                var fields = _sudokuService.InitFields();
                var preDefinedFields = await _sudokuService.GetSudokuGameFromWebApi(GameDifficulty.Easy);
                if (preDefinedFields.Any())
                {
                    fields = _sudokuService.ReplacePreDefinedFields(fields, preDefinedFields);
                    EventAggregator.GetEvent<SudokuFieldLoadedEvent>().Publish(fields);
                }

            }
            IsBusy = false;
        }

        private void TileClick(Field field)
        {
            // Maybe use that one later
        }

        private async void Reload()
        {
            OutputText = "";
            await InitGame();
        }

        private void Validate()
        {
            OutputText = "";
            
            if (Fields.Any(f => !f.Predefined))
            {
                foreach (var customField in Fields.Where(f => !f.Predefined && f.Value != -1))
                {
                    var columnFieldsRow = Fields.Where(f => f.X == customField.X && f.Y != customField.Y);
                    var columnFieldsColumn = Fields.Where(f => f.Y == customField.Y && f.X != customField.X);
                    if (columnFieldsColumn.Any(f => f.Value == customField.Value) ||
                        columnFieldsRow.Any(f => f.Value == customField.Value))
                    {
                        OutputText += Environment.NewLine;
                        OutputText += "Du hast eine ungültige Eingabe in der Spalte oder Reihe!";
                        return;
                    }
                }
            }
            if (Fields.Any(f => f.Value == -1))
            {
                OutputText = "Nicht alle Felder ausgefüllt...";
                OutputText += Environment.NewLine;
                OutputText += "Aber bis dahin sieht es gut aus!";
            }
            else
            {
                OutputText += Environment.NewLine;
                OutputText += "WOW Gratuliere, Du bist fertig!";
            }
        }
    }
}
