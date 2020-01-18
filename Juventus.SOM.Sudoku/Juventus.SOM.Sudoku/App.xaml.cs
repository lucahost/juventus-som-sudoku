using Juventus.SOM.Sudoku.Services;
using Juventus.SOM.Sudoku.ViewModels;
using Juventus.SOM.Sudoku.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace Juventus.SOM.Sudoku
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }
        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterSingleton<ISudokuService, SudokuService>();
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
