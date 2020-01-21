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
		/// <summary>Class <c>LevelOneModel</c>
		/// Contains the logic and functionality of the first and seconed level of the game
		/// </summary>
		private string _guessesString;
		private HashSet<char> _wordLetters;
		public LevelOneModel(string word, TimeModel timer, double percentage, int score) : base(word, timer,percentage,score)
		{
			_wordLetters = getWordLetters(word.ToLower());
		}
		///<summary>
		///see if the player guessed all the letters of the word
		///</summary>
		private bool isSolved()
		{
			foreach (var letter in this._wordLetters)
				if (!this._guesses.Contains(Char.ToLower(letter)))
					return false;
			return true;
		}
		///<summary>
		///Take the player letter guess, update the list of guesses,
		///check if player got all letters of the word.
		///</summary>
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
					// add to guess the new letter and print the update the hidde word
					this._guesses.Add(lower);
					updateHiddenWord();
					updateGuessesString();
					if (isSolved())
						gameOver();
				}
			}
			// if there is an exception the input is invalid, we ignore it
			catch (ArgumentNullException) { }
			catch (FormatException) { }
		}

		public override void start()
		{
			base.start();
			updateGuessesString();
		}
		///<summary>
		///Update the list of gueesed letters that the player sees
		///</summary>
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
