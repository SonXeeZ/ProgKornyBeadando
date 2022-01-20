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

namespace PKBEAD
{
    public partial class MainWindow : Window
    {

        private Hangman Game { get; set; }
        private List<Button> Letters { get; set; }
        private List<Label> Labels { get; set; }
        private List<string> Words { get; set; }
        private Image StageImage { get; set; }
        private int WinStreak { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Labels = new List<Label>(); 
            Letters = new List<Button>(); 
            Words = new List<string>(); // Szavak a Dictionary.txt-ből.
            WinStreak = 0;

            LoadWords();
            CreateNewGameBtn();
        }

        private void LoadWords() // Dictionary.txt betöltése.
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("dictionary.txt");
            while ((line = file.ReadLine()) != null)
            {
                Words.Add(line);
            }
            file.Close();
        }

        private void NewGameBtnClick(object sender, RoutedEventArgs e) // Új játék
        {

            Random r = new Random(); 
            InitializeGameField(Words[r.Next(0, Words.Count)]);
        }

        private void CharacterBtnClick(object sender, RoutedEventArgs e) 
        {
            int[] temp = Game.CheckLetter((sender as Button).Content.ToString()[0]);

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == 1)
                {
                    Labels[i].Content = Game.Word[i];
                }
            }

            StageImage.Source = Game.CurrentStageImage(); // Kép beállítása.

            if (Labels.Count(l => l.Content == null) == 0)
            {
                FinishGame("You Win!");
                WinStreak++;
            }
            else if (Game.IsGameOver())
            {
                FinishGame("You Lose!");
                WinStreak = 0;
            }
            else
            {
                (sender as Button).IsEnabled = false; // Betűk kikapcsolása, amik már használva lettek.
            }
        }

        private void FinishGame(string message) 
        {
            MessageBox.Show(message);
            Letters.ForEach(b => b.IsEnabled = false);
        }

        private void InitializeGameField(string word) // Inicializálások
        {
            Game = new Hangman(word);

            Labels.Clear();
            Letters.Clear();
            GameField.Children.Clear();

            CreateImage();
            StageImage.Source = Game.CurrentStageImage();

            CreateNewGameBtn();
            CreateCharacterBtns(Game.Alphabet);
            CreateCharacterLbl(Game.Lenght);
            CreateWinstreakLbl();
        }


        
        private void CreateNewGameBtn() 
        {
            Button button = new Button();
            button.VerticalAlignment = VerticalAlignment.Center;
            button.HorizontalAlignment = HorizontalAlignment.Right;
            button.Width = 100;
            button.Height = 35;

            button.Content = "New Game";
            button.Click += new RoutedEventHandler(NewGameBtnClick);

            GameField.Children.Add(button);
        }
        private void CreateImage()
        {
            StageImage = new Image();

            StageImage.Name = "StageImage";
            StageImage.VerticalAlignment = VerticalAlignment.Center;
            StageImage.HorizontalAlignment = HorizontalAlignment.Center;
            StageImage.Width = 150;
            StageImage.Height = 150;

            GameField.Children.Add(StageImage);
        }

        private void CreateWinstreakLbl()
        {
            Label WinstreakLbl = new Label();
            WinstreakLbl.FontSize = 15;
            WinstreakLbl.FontWeight = FontWeight;
            WinstreakLbl.HorizontalContentAlignment = HorizontalAlignment.Center;
            WinstreakLbl.VerticalContentAlignment = VerticalAlignment.Center;
            WinstreakLbl.Height = 40;
            WinstreakLbl.Width = 170;
            WinstreakLbl.HorizontalAlignment = HorizontalAlignment.Center;
            WinstreakLbl.VerticalAlignment = VerticalAlignment.Top;

            WinstreakLbl.Margin = new Thickness(0d, 40d, 0d, 0d);

            WinstreakLbl.Content = "Current Winstreak: " + WinStreak;

            GameField.Children.Add(WinstreakLbl);

        }

        private void CreateCharacterLbl(int lenght)
        {
            for (int i = 0; i < lenght; i++)
            {
                Label label = new Label();
                label.FontSize = 20;
                label.FontWeight = FontWeight;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.BorderThickness = new Thickness(1, 1, 1, 1);
                label.BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x2D, 0x2D, 0x30));
                label.Height = label.Width = 38;
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Top;

                label.Name = "Character" + i.ToString();

                label.Margin = new Thickness(i * label.Width, 0d, 0d, 0d);

                Labels.Add(label);

                GameField.Children.Add(label);
            }
        }

        private void CreateCharacterBtns(char[] alph) 
        {
            double bot = 0;
            int count = 0;
            for (int i = 0; i < alph.Length; i++, count++)
            {
                Button button = new Button();
                button.FontSize = 20;
                button.FontWeight = FontWeight;
                button.HorizontalContentAlignment = HorizontalAlignment.Center;
                button.VerticalContentAlignment = VerticalAlignment.Center;
                button.Height = button.Width = 38;
                button.HorizontalAlignment = HorizontalAlignment.Left;
                button.VerticalAlignment = VerticalAlignment.Bottom;

                button.Content = alph[i].ToString();

                if ((count + 1) * button.Width > GameField.Width)
                {
                    count = 0;
                    bot += button.Height;
                }

                button.Margin = new Thickness(count * button.Width, 0, 0, bot);
                button.Click += new RoutedEventHandler(CharacterBtnClick);

                Letters.Add(button);

                GameField.Children.Add(button);
            }
        }
    }
}
