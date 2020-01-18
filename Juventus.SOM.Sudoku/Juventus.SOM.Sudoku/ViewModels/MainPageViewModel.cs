using System;
using System.Collections.Generic;
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
        private DelegateCommand<int> _buttonActionCommand;
        public ICommand ButtonActionCommand =>
            _buttonActionCommand ??
            (_buttonActionCommand = new DelegateCommand<int>(ButtonAction));

        private void ButtonAction(int i)
        {

        }
        private ISudokuService _sudokuService;
        private string _title;
        public string Title {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private IEnumerable<Field> _preDefinedFields;
        public IEnumerable<Field> PreDefinedFields {
            get => _preDefinedFields;
            set => SetProperty(ref _preDefinedFields, value);
        }


        public MainPageViewModel(INavigationService navigationService, ISudokuService sudokuService)
            : base(navigationService)
        {
            Title = "Main Page";
            _sudokuService = sudokuService;
        }
        public void OnNavigatedFrom(NavigationParameters parameters) { }

        public override async void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            base.OnNavigatedTo(navigationParameters);
            if (_sudokuService != null)
            {
                PreDefinedFields = await _sudokuService.GetSudokuGameFromWebApi(GameDifficulty.Easy);
            }
        }
    }
}
