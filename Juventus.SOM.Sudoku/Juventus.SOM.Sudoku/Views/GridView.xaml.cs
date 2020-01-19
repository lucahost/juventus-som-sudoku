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
                        if (item is Grid grid)
                        {
                            if (grid.Children.Any())
                            {
                                grid.Children.Clear();
                            }
                        }
                    }
                }

                // Wipe out the previous row definitions if they're there.
                //RowDefinitions?.Clear();

                var enumerable = tiles as IList ?? tiles.ToList();

                //for (var i = 0; i < numberOfRows; i++)
                //    RowDefinitions?.Add(new RowDefinition { Height = GridLength.Star });

                for (var index = 0; index < enumerable.Count; index++)
                {
                    var tile = BuildTile(enumerable[index]);
                    var item = enumerable[index];
                    if (item is Field field)
                    {
                        if (field.X < 3 && field.Y < 3)
                        {
                            Grid00.Children.Add(tile, field.Y, field.X);
                            continue;
                        }
                        if (field.X < 3 && field.Y >= 3 && field.Y < 6)
                        {
                            Grid01.Children.Add(tile, field.Y - 3, field.X);
                            continue;
                        }
                        if (field.X < 3 && field.Y >= 6 && field.Y < 9)
                        {
                            Grid02.Children.Add(tile, field.Y - 6, field.X);
                            continue;
                        }

                        if (field.X >= 3 && field.X < 6 && field.Y < 3)
                        {
                            Grid10.Children.Add(tile, field.Y, field.X - 3);
                            continue;
                        }
                        if (field.X >= 3 && field.X < 6 && field.Y >= 3 && field.Y < 6)
                        {
                            Grid11.Children.Add(tile, field.Y - 3, field.X - 3);
                            continue;
                        }
                        if (field.X >= 3 && field.X < 6 && field.Y >= 6 && field.Y < 9)
                        {
                            Grid12.Children.Add(tile, field.Y - 6, field.X - 3);
                            continue;
                        }

                        if (field.X >= 6 && field.X < 9 && field.Y < 3)
                        {
                            Grid20.Children.Add(tile, field.Y, field.X - 6);
                            continue;
                        }
                        if (field.X >= 6 && field.X < 9 && field.Y >= 3 && field.Y < 6)
                        {
                            Grid21.Children.Add(tile, field.Y - 3, field.X - 6);
                            continue;
                        }
                        if (field.X >= 6 && field.X < 9 && field.Y >= 6 && field.Y < 9)
                        {
                            Grid22.Children.Add(tile, field.Y - 6, field.X - 6);
                            continue;
                        }
                    }
                    else
                    {
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