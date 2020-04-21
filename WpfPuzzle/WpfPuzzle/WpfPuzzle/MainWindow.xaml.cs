﻿using System;
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
        }
    }
}
