using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Juventus.SOM.Sudoku.Core;
using Juventus.SOM.Sudoku.Models;
using Juventus.SOM.Sudoku.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace Juventus.SOM.Sudoku.ViewModels
{
    internal class MainPageViewModel : ViewModelBase
    {
        private string _title;
        private readonly ISudokuService _sudokuService;
        private List<Field> _fields;

        public string Title {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public List<Field> Fields {
            get => _fields;
            set => SetProperty(ref _fields, value);
        }

        public MainPageViewModel(INavigationService navigationService, ISudokuService sudokuService)
            : base(navigationService)
        {
            Title = "Main Page";
            _sudokuService = sudokuService;
        }

        public override async void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            base.OnNavigatedTo(navigationParameters);
            if (_sudokuService != null)
            {
                Fields = _sudokuService.InitFields();
                var preDefinedFields = await _sudokuService.GetSudokuGameFromWebApi(GameDifficulty.Easy);
                if (preDefinedFields.Any())
                {
                    _sudokuService.ReplacePreDefinedFields(Fields, preDefinedFields);
                }
            }
        }
    }
}
