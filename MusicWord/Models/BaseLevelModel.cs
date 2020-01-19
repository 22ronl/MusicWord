using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
namespace MusicWord.Models
{
    class BaseLevelModel: PropertyChangedBase
	{
		public event EventHandler GameOver;
		protected double _percentage;
		protected HashSet<char> _guesses;
		protected string _word;
		protected TimeModel _timer;
		protected int _score;
		// word that changes on the screen
		protected string _hiddenWord;
		public BaseLevelModel(string word, TimeModel timer, double percentage, int score)
		{
			_score = score;
			_percentage = percentage;
			_guesses = creatDeafultGuesses(word);
			_word = word;
			// to show the letters to the player
			_timer = timer;
			_timer.TimeOver += timeIsOver;
			_timer.start();
		}
		public string HiddenWord
		{
			get { return _hiddenWord; }
			set
			{
				_hiddenWord = value;
				NotifyOfPropertyChange(() => HiddenWord);
			}
		}
		protected void timeIsOver(object sender, EventArgs e)
		{
			gameOver();
		}
		protected string shuffle(string str)
		{
			char[] array = str.ToCharArray();
			Random rng = new Random();
			int n = array.Length;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				var value = array[k];
				array[k] = array[n];
				array[n] = value;
			}
			return new String(array);
		}
		protected HashSet<char> getWordLetters(string word)
		{
			var letters = new HashSet<char>();
			foreach (var letter in word)
				if (Char.IsLetter(letter))
				{
					letters.Add(letter);
				}
			return letters;
		}

		protected HashSet<char> creatDeafultGuesses(string word)
		{
			var letters = new HashSet<char>();
			var showenLetters = new HashSet<char>();
			int numOfShowenLetters;
			// get shuffle word array
			string shuffleWord = shuffle(word.ToLower());

			// create set of word letters
			letters = getWordLetters(shuffleWord);
			numOfShowenLetters = (int)Math.Ceiling(this._percentage * letters.Count);

			// create set of only showen letters
			int i = 0;
			foreach (var letter in letters)
			{
				if (i < numOfShowenLetters)
					showenLetters.Add(letter);
				else
					break;
				i++;
			}
			return showenLetters;

		}
		protected void updateHiddenWord()
		{
			string wordString = "";
			foreach (var letter in this._word)
			{
				if (letter == ' ')
					wordString += "   ";
				else if (this._guesses.Contains(Char.ToLower(letter)) || !Char.IsLetter(letter))
					wordString += letter + " ";
				else
					wordString += "__ ";
			}
			HiddenWord = wordString;
		}

		public virtual void start()
		{
			updateHiddenWord();
		}

		public void EnterWord(string word)
		{
			if (word.ToLower() == this._word.ToLower())
			{
				Score += Globals.wordScore;
				gameOver();
			}
		}
			
		protected void gameOver()
		{
			Score += _timer.Secondes * Globals.secondsScore;
			PlayerModel.Instance.Score = Score;
			_timer.stop();
			GameOver(this, EventArgs.Empty);
		}
		public int Score
		{
			get { return _score; }
			set
			{
				_score = value;
				NotifyOfPropertyChange(() => Score);
			}
		}

	}
}
