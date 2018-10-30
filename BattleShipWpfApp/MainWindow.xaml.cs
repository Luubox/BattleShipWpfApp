using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleShipWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _missCounter = 1;
        int gridSize = 10;
        String[,] _gridArray;
        Button[,] _buttonArray;
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            _gridArray = new String[gridSize, gridSize];
            _buttonArray = new Button[gridSize, gridSize];

            CreateShip(2);
            CreateShip(3);
            CreateShip(4);
            CreateShip(5);

            for (int i = 0; i < gridSize; i++)
            {
                RowDefinition rd = new RowDefinition(); //generating grid/row defitions automatically per grid size
                rd.Height = new GridLength(50, GridUnitType.Pixel);
                ViewGrid.RowDefinitions.Add(rd);

                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(50, GridUnitType.Pixel);
                ViewGrid.ColumnDefinitions.Add(cd);
            }

            for (int x = 0; x < gridSize; x++) //>>
            {
                for (int y = 0; y < gridSize; y++) //VV
                {
                    //2x1, 3x1, 4x1, 5x1 no overlap, no diagonal


                    Button btn = new Button();
                    btn.Click += GridClicked(x, y);
                    btn.Content = "btn";
                    _buttonArray[x, y] = btn;
                    btn.Margin = new Thickness(2, 2, 2, 2);
                    ViewGrid.Children.Add(btn);
                    Grid.SetColumn(btn, x);
                    Grid.SetRow(btn, y);
                }
            }
        }

        private void CreateShip(int size)
        {
            var randomYIndex = random.Next(0, gridSize); //random.Next(min,max) min included, max excluded
            var randomXIndex = random.Next(0, gridSize);
            bool direction = random.NextDouble() >= 0.5;
            List<Point> shipList = new List<Point>();
            CheckShip(size, randomYIndex, randomXIndex, direction, shipList);
        }

        private void CheckShip(int size, int randomYIndex, int randomXIndex, bool direction, List<Point> shipList)
        {
            if (!shipList.Contains(new Point(randomXIndex, randomYIndex))) //TODO virker ikke
            {
                if ((direction && randomXIndex <= (gridSize - 1) - size) || (!direction && randomYIndex <= (gridSize - 1) - size))
                {
                    for (int i = 0; i < size; i++)
                    {
                        if (direction) shipList.Add(new Point(randomXIndex + i, randomYIndex));
                        else shipList.Add(new Point(randomXIndex, randomYIndex + i));

                        //if (direction) _gridArray[randomXIndex + i, randomYIndex] = $"{size}"; //right
                        //else _gridArray[randomXIndex, randomYIndex + i] = $"{size}"; //down
                    }

                    foreach (var ship in shipList)
                    {
                        int x = Convert.ToInt32(ship.X);
                        int y = Convert.ToInt32(ship.Y);
                        _gridArray[x, y] = "Hit";
                    }
                }
            }
            else
                CreateShip(size);
        }

        private RoutedEventHandler GridClicked(int x, int y) //click event applied to all buttons via for loop
        {
            return (btn, e) =>
            {
                //RevealAll();

                _buttonArray[x, y].IsEnabled = false;

                _buttonArray[x, y].Content = _gridArray[x, y];
                if (_gridArray[x, y] != "Hit")
                {
                    ScoreBoard.Text = $"Number of misses: {_missCounter++.ToString()}";
                }
            };
        }

        public void RevealAll()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    _buttonArray[x, y].Content = _gridArray[x, y];
                }
            }
        }
    }
}
