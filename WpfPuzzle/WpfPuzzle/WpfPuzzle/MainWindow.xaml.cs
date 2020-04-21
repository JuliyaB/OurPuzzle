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

namespace WpfPuzzle
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Puzzle puzzle = new Puzzle();
            
            ObservableCollection<PuzzlePiece> itemPlacement = new ObservableCollection<PuzzlePiece>();
            
            PuzzlePiece emptyItem = new PuzzlePiece();
            
            ListBox lbDragSource;
            
            Canvas cvDragSource;
            public MainWindow()
            {
                InitializeComponent();
                
                puzzle.Initialize(1);
                emptyItem.index = -1;
                emptyItem.PuzzleImageSource = new BitmapImage();
                emptyItem.UriString = "";
                for (int i = 0; i < 9; i++)
                {
                    itemPlacement.Add(emptyItem);
                    
                    itemPlacement[i].index = i;
                }
                puzzleItemList.ItemsSource = puzzle.puzzlePiece;
                puzzle.Edited += new EventHandler(puzzle_Edited);
            }

            private void puzzleItemList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = sender as ListBox;
            lbDragSource = parent;
            object data = GetObjectDataFromPoint(lbDragSource, e.GetPosition(parent));

            if (data != null)
            {
                PuzzlePiece itemSelected = data as PuzzlePiece;
                itemSelected.DragFrom = typeof(ListBox);
                DragDrop.DoDragDrop(lbDragSource, data, DragDropEffects.Move);
            }
        }
            private void PzItmCvs_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas parent = sender as Canvas;
            cvDragSource = parent;
            object data = GetDataFromCanvas(cvDragSource);

            if (data != null)
            {
                PuzzlePiece itemSelected = data as PuzzlePiece;
                itemSelected.DragFrom = typeof(Canvas);
                DragDrop.DoDragDrop(cvDragSource, data, DragDropEffects.Move);
            }
        }
            private void PuzzleItemDrop(object sender, DragEventArgs e)
        {
            Canvas destination = sender as Canvas;
            PuzzlePiece itemTransferred = null;
            object data = e.Data.GetData(typeof(PuzzlePiece)) as PuzzlePiece;
            itemTransferred = data as PuzzlePiece;

            Image imageControl = new Image()
            {
                Width = destination.Width,
                Height = destination.Height,
                Source = itemTransferred.PuzzleImageSource,
                Stretch = Stretch.UniformToFill
            };

            if (destination.Children.Count == 0)
            {
                destination.Children.Add(imageControl);
                int indexToUpdate = int.Parse(destination.Tag.ToString());
                if (itemTransferred.DragFrom == typeof(ListBox))
                {
                    this.itemPlacement[indexToUpdate] = itemTransferred;
                    ((IList)lbDragSource.ItemsSource).Remove(itemTransferred);
                }
                else if (itemTransferred.DragFrom == typeof(Canvas))
                {
                    int previousIndex = itemPlacement.IndexOf(itemTransferred);
                    itemPlacement[indexToUpdate] = itemTransferred;
                    itemPlacement[previousIndex] = emptyItem;

                    Canvas associated = GetAssociatedCanvasByIndex(previousIndex);
                    associated.Children.Clear();
                    associated = null;
                }
                else
                {
                    MessageBox.Show("Dragsource не из listbox или canvas");
                }
            }
            else if (destination.Children.Count > 0)
            {
                if (itemTransferred.DragFrom == typeof(ListBox))
                {
                    return;
                }
                else if (itemTransferred.DragFrom == typeof(Canvas))
                {
                    int sourceIndex = itemPlacement.IndexOf(itemTransferred);
                    int destinationIndex = int.Parse(destination.Tag.ToString());

                    Object buffer = null;
                    Image image0 = new Image() { Width = destination.Width, Height = destination.Height, Stretch = Stretch.Fill };
                    image0.Source = itemPlacement[sourceIndex].PuzzleImageSource;

                    Image image1 = new Image() { Width = destination.Width, Height = destination.Height, Stretch = Stretch.Fill };
                    image1.Source = itemPlacement[destinationIndex].PuzzleImageSource;

                    GetAssociatedCanvasByIndex(sourceIndex).Children.Clear();
                    GetAssociatedCanvasByIndex(destinationIndex).Children.Clear();
                    GetAssociatedCanvasByIndex(sourceIndex).Children.Add(image1);
                    GetAssociatedCanvasByIndex(destinationIndex).Children.Add(image0);

                    image0 = null;
                    image1 = null;

                    buffer = itemPlacement[sourceIndex];

                    itemPlacement[sourceIndex] = itemPlacement[destinationIndex];
                    itemPlacement[destinationIndex] = buffer as PuzzlePiece;

                    buffer = null;
                }
            }
            puzzle.OnEdit(EventArgs.Empty);
            itemTransferred.DragFrom = null;
        }
            void puzzle_Edited(object sender, EventArgs e)
        {
            bool validate = puzzle.Validate(itemPlacement);

            if (validate)
            {
                MessageBox.Show("Поздравляю. Вы собрали пазл!");
            }
        }

        }
    }
}
