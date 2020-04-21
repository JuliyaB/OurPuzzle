using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfPuzzle
{
    class Puzzle
    {
        public ObservableCollection<PuzzlePiece> puzzlePiece = new ObservableCollection<PuzzlePiece>();

        public string name;

        public event EventHandler Edited;

        public Puzzle()
        {

        }

        public void OnEdit(EventArgs e)
        {
            if (Edited != null)
                Edited(this, e);
        }

        public void Initialize(int chosen)
        {
            string directorySource = "";

            if (chosen == 1)
            {
                this.name = "Witcher Puzzle";

                directorySource = "WitcherP";
            }

            for (int i = 0; i < 9; i++)
            {
                this.puzzlePiece.Add(new PuzzlePiece());

                this.puzzlePiece[i].index = i;

                this.puzzlePiece[i].UriString = "Puzzle/" + directorySource + "/" + (i + 1).ToString() + ".jpg";

                this.puzzlePiece[i].PuzzleImageSource = new BitmapImage(new Uri(this.puzzlePiece[i].UriString, UriKind.Relative));
            }

            Random rand = new Random();

            for (int i = 0; i < 9; i++)
            {
                int random = rand.Next(0, 8);

                PuzzlePiece buffer;

                buffer = this.puzzlePiece[i];

                this.puzzlePiece[i] = this.puzzlePiece[random];

                this.puzzlePiece[random] = buffer;
            }
        }

        public bool Validate(ObservableCollection<PuzzlePiece> itemPlacement)
        {
            ObservableCollection<PuzzlePiece> placement = itemPlacement;

            foreach (PuzzlePiece item in placement)
            {
                if ((placement.IndexOf(item) != item.index) || placement.IndexOf(item) < 0)

                    return false;
            }

            return true;
        }
    }
}
