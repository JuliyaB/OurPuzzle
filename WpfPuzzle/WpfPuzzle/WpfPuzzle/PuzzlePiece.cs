using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfPuzzle
{
    class PuzzlePiece
    {
        public int index = -1;

        public ImageSource PuzzleImageSource { get; set; }

        public string UriString { get; set; }

        public Type DragFrom { get; set; }
    }
}
