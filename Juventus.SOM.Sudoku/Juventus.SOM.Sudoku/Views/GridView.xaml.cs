using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Juventus.SOM.Sudoku.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Juventus.SOM.Sudoku.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GridView
    {
        public GridView()
        {
            InitializeComponent();

            for (var i = 0; i < MaxColumns; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create<GridView, object>(
            p => p.CommandParameter,
            null);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create<GridView, ICommand>(
            p => p.Command,
            null);
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<GridView, IEnumerable<object>>(p => p.ItemsSource,
                null,
                BindingMode.OneWay,
                null,
                (bindable,
                    oldValue,
                    newValue) =>
                {
                    ((GridView)bindable).BuildTiles(newValue);
                });

        public Type ItemTemplate { get; set; } = typeof(SudokuCellTemplate);


        private int _maxColumns = 9;
        public int MaxColumns {
            get => _maxColumns;
            set => _maxColumns = value;
        }

        public object CommandParameter {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public ICommand Command {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public IEnumerable<object> ItemsSource {
            get => (IEnumerable<object>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public void BuildTiles(IEnumerable<object> tiles)
        {
            try
            {
                var items = Children?.ToList();
                if (items?.Any() == true)
                {
                    foreach (var item in items)
                    {
                        Children.Remove(item);
                    }
                }

                // Wipe out the previous row definitions if they're there.
                RowDefinitions?.Clear();

                var enumerable = tiles as IList ?? tiles.ToList();
                var numberOfRows = Math.Ceiling(enumerable.Count / (float)MaxColumns);

                for (var i = 0; i < numberOfRows; i++)
                    RowDefinitions?.Add(new RowDefinition { Height = GridLength.Star });

                for (var index = 0; index < enumerable.Count; index++)
                {
                    var column = index % MaxColumns;
                    var row = (int)Math.Floor(index / (float)MaxColumns);

                    var tile = BuildTile(enumerable[index]);
                    var item = enumerable[index];
                    if (item is Field field)
                    {
                        Children?.Add(tile, field.Y - 1, field.X - 1);
                    }
                    else
                    {
                        Children?.Add(tile, column, row);
                    }
                }
            }
            catch
            { // can throw exceptions if binding upon disposal
            }
        }

        private Layout BuildTile(object item1)
        {
            var buildTile = (Grid)Activator.CreateInstance(ItemTemplate, item1);

            //if (item1 is Field field)
            //{
            //    if (field.X % 3 == 0 && field.Y % 3 == 0)
            //    {
            //        var boxView = new BoxView
            //        {
            //            BackgroundColor = Color.Black,
            //            Color = Color.Red,
            //            HeightRequest = 0,
            //            WidthRequest = 1
            //        };
            //        buildTile.Children.Insert(1, boxView);
            //    }
            //    else if (field.X % 3 == 0)
            //    {
            //        var boxView = new BoxView
            //        {
            //            BackgroundColor = Color.Black,
            //            Color = Color.Red,
            //            HeightRequest = 0,
            //            WidthRequest = 1
            //        };
            //        buildTile.Children.Insert(1, boxView);
            //    }
            //    else if (field.Y % 3 == 0)
            //    {
            //        var boxView = new BoxView
            //        {
            //            BackgroundColor = Color.Black,
            //            Color = Color.Red,
            //            WidthRequest = 1,
            //            HeightRequest = 0
            //        };
            //        buildTile.Children.Insert(1, boxView);
            //    }
            //}

            buildTile.InputTransparent = false;

            var tapGestureRecognizer = new TapGestureRecognizer
            {
                Command = Command,
                CommandParameter = item1,
                NumberOfTapsRequired = 1
            };

            buildTile?.GestureRecognizers.Add(tapGestureRecognizer);


            return buildTile;
        }
    }
}