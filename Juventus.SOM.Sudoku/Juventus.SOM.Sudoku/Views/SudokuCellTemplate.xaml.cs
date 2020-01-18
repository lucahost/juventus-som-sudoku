using Xamarin.Forms.Xaml;

namespace Juventus.SOM.Sudoku.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SudokuCellTemplate
    {
        public SudokuCellTemplate()
        {
            InitializeComponent();
        }

        public SudokuCellTemplate(object item)
        {
            InitializeComponent();
            BindingContext = item;
        }
    }
}