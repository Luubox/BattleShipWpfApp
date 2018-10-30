using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
        private int _hitCounter = 0;
        int gridSize = 10;
        String[,] _gridArray;
        Button[,] _buttonArray;
        private Random random = new Random();
        List<Point> shipList = new List<Point>();

        public MainWindow()
        {
            InitializeComponent();
            _gridArray = new String[gridSize, gridSize];
            _buttonArray = new Button[gridSize, gridSize];

            CreateGrid();
            CreateButtons();

            CreateShip(2, shipList);
            CreateShip(3, shipList);
            CreateShip(3, shipList);
            CreateShip(4, shipList);
            CreateShip(5, shipList);
        }

        private void CreateButtons()
        {
            for (int x = 0; x < gridSize; x++) //>>
            {
                for (int y = 0; y < gridSize; y++) //VV
                {
                    Button btn = new Button();
                    btn.Click += GridClicked(x, y);
                    btn.Content = "";
                    _buttonArray[x, y] = btn;
                    btn.Margin = new Thickness(2, 2, 2, 2);
                    ViewGrid.Children.Add(btn);
                    Grid.SetColumn(btn, x);
                    Grid.SetRow(btn, y);
                }
            }
        }

        private void CreateGrid()
        {
            for (int i = 0; i < gridSize; i++)
            {
                RowDefinition rd = new RowDefinition(); //generating grid/row defitions automatically per grid size
                rd.Height = new GridLength(50, GridUnitType.Pixel);
                ViewGrid.RowDefinitions.Add(rd);

                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(50, GridUnitType.Pixel);
                ViewGrid.ColumnDefinitions.Add(cd);
            }
        }

        private void CreateShip(int size, List<Point> shipList)
        {
            var randomYIndex = random.Next(0, gridSize); //random.Next(min,max) min included, max excluded
            var randomXIndex = random.Next(0, gridSize);

            bool direction = random.NextDouble() >= 0.5;
            bool shipAhoy = true;

            CheckShip(size, randomYIndex, randomXIndex, direction, shipList, shipAhoy);
        }

        private void CheckShip(int size, int randomYIndex, int randomXIndex, bool direction, List<Point> shipList, bool shipAhoy)
        {
            if ((direction && randomXIndex <= (gridSize - 1) - size) || (!direction && randomYIndex <= (gridSize - 1) - size))
            {
                if (direction)
                {
                    for (int i = 0; i < size; i++)
                    {
                        if (shipList.Contains(new Point(randomXIndex + i, randomYIndex)))
                        {
                            shipAhoy = false;
                        }
                    }

                    if (shipAhoy)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            shipList.Add(new Point(randomXIndex + j, randomYIndex));
                        }
                    }
                    else
                        CreateShip(size, shipList);
                }
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        if (shipList.Contains(new Point(randomXIndex, randomYIndex + i)))
                        {
                            shipAhoy = false;
                        }
                    }

                    if (shipAhoy)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            shipList.Add(new Point(randomXIndex, randomYIndex +j));
                        }
                    }
                    else
                        CreateShip(size, shipList);
                }
                foreach (var ship in shipList)
                {
                    int x = Convert.ToInt32(ship.X);
                    int y = Convert.ToInt32(ship.Y);
                    _gridArray[x, y] = /*size.ToString()*/ "Hit";
                }
            }
            else
                CreateShip(size, shipList);
        }

        private RoutedEventHandler GridClicked(int x, int y) //click event applied to all buttons via for loop
        {
            return (btn, e) =>
            {
                _buttonArray[x, y].IsEnabled = false;
                _buttonArray[x, y].Content = _gridArray[x, y];

                if (_gridArray[x, y] != "Hit") ScoreBoard.Text = $"Number of misses: {_missCounter++.ToString()}";
                else
                {
                    SoundPlayer player = new SoundPlayer(@"Sounds\Bomb3.wav");
                    player.Play();
                    _hitCounter++;
                }

                if (_hitCounter == shipList.Count) RevealAll();
            };
        }

        public void RevealAll()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    _buttonArray[x, y].Content = _gridArray[x, y];
                    _buttonArray[x, y].IsEnabled = false;
                }
            }
        }
    }
}
