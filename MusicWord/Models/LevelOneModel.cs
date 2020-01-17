using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MusicWord.Models
{
	internal class LevelOneModel : BaseLevelModel
	{
		private string _guessesString;
		private HashSet<char> _wordLetters;
		public LevelOneModel(string word, TimeModel timer, double percentage, int score) : base(word, timer,percentage,score)
		{
			_wordLetters = getWordLetters(word.ToLower());
		}

		private bool isSolved()
		{
			foreach (var letter in this._wordLetters)
				if (!this._guesses.Contains(Char.ToLower(letter)))
					return false;
			return true;
		}

		public void EnterGuess(string guess)
		{
			try
			{
				// check if input is valid
				char letter = char.Parse(guess);
				if (Char.IsLetter(letter))
				{
					char lower = Char.ToLower(letter);
					if (_wordLetters.Contains(lower) && !_guesses.Contains(lower))
						Score += Globals.letterScore;
					// add to guess the new letter and print the word
					this._guesses.Add(lower);
					updateHiddenWord();
					updateGuessesString();
					if (isSolved())
						gameOver();
				}
			}
			// if there is an exception the input is not valid we ignore it
			catch (ArgumentNullException) { }
			catch (FormatException) { }
		}

		public override void start()
		{
			base.start();
			updateGuessesString();
		}
		private void updateGuessesString()
		{
			string guesses = "";
			foreach (var letter in _guesses)
			{

				guesses += Char.ToString(letter) + ",";
			}
			guesses = guesses.Remove(guesses.Length - 1);
			Guesses = guesses;
		}


		public string Guesses
		{
			get { return _guessesString; }
			set
			{
				_guessesString = value;
				NotifyOfPropertyChange(() => Guesses);
			}
		}

	}
}
