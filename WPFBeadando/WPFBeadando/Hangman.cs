using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PKBEAD
{
    class Hangman
    {
        public string Word { get; private set; }
        public int Lenght { get; private set; }
        private int Stage { get; set; }
        public char[] Alphabet { get; private set; }


        public Hangman(string wordToGuess)
        {
            Word = wordToGuess;
            Lenght = Word.Length;
            Stage = 0;
            InitializeAlphabet();

        }

        private void InitializeAlphabet()
        {
            Alphabet = new char[] { 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };
        }

        public BitmapImage CurrentStageImage() //Bitmapimage segítségével megjelenítjük a külömböző "szinteket".
        {
            Uri path = new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Stages", "s" + Stage + ".png"));
            BitmapImage bi = new BitmapImage(path);

            return bi;
        }

        public int[] CheckLetter(char ch)
        {
            int[] temp = new int[Word.Length];

            for (int i = 0; i < Word.Length; i++)
            {
                if (Word.ToUpper()[i] == ch) // Ha megegyezik a betű, a szóban lévő betűvel, beír egy '1'-est.
                {
                    temp[i] = 1;
                }
                else
                {
                    temp[i] = 0;
                }
            }

            if (temp.Count(i => i == 1) == 0) // Leellenőrzi hogy a tempben van-e '1'-es, ha nem akkor nő a "szint".
            {
                Stage++;
            }

            return temp;
        }

        public bool IsGameOver() // Leellenőrzi hogy véget ért-e már a játék.
        {
            bool gameover = false;

            if (Stage == 6)
                gameover = true;

            return gameover;
        }
    }
}

