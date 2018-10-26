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

        int gridSize = 10;
        String[,] _gridArray;
        Button[,] _buttonArray;
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            _gridArray = new String[gridSize, gridSize];
            _buttonArray = new Button[gridSize, gridSize];

            var randomXIndex = random.Next(0, gridSize-1);
            var randomYIndex = random.Next(0, gridSize-1);
            _gridArray[randomXIndex, randomYIndex] = "test";

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

        private RoutedEventHandler GridClicked(int x, int y) //click event applied to all buttons via for loop
        {
            return (btn, e) =>
            {
                RevealAll();
                _buttonArray[x, y].Content = _gridArray[x, y];
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
