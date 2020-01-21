using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicWord.Models;
using System.Reflection;
using System.Windows;

namespace MusicWord.ViewModels
{
	class LevelOneViewModel : BaseLevelViewModel
	{
		/// <summary>Class <c>BaseLevelViewModel</c>
		/// Create the game calss with needed parameters
		/// adding guesses letters functionality for the player
		/// </summary>

		protected string _letterGuess;
		protected string _guesses;
		protected LevelOneModel _game;
		public LevelOneViewModel()
		{
			// get the hidden word
			ICategory category = SQLServerModel.Instance.getWord(PlayerModel.Instance.Category);
			_cluesGenrator = new CluesModel(category);
			string word = category.Name;
			// register for time notifications 
			_timer = new TimeModel(Globals.levelTime);
			_timer.PropertyChanged += _timer_PropertyChanged;
			// create game with current plater score
			int score = PlayerModel.Instance.Score;
			_game = new LevelOneModel(word, _timer, Globals.hidddenPercentage,score);
			_game.PropertyChanged += _game_PropertyChanged;
			_game.GameOver += _game_GameOver;
			_game.start();
			baseBuilder(_game, score,word);
		}
		///<summary>
		///Update view on changes in the game
		///</summary>
		public override void _game_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Globals.hiddenWord)
			{
				HiddenWord = _game.HiddenWord;
			}
			else if (e.PropertyName == Globals.guesses)
			{
				Guesses = _game.Guesses;
			}
			else if (e.PropertyName == Globals.score)
			{
				Score= _game.Score;
			}
		}

		public string Guesses
		{
			get { return _guesses; }
			set
			{
				_guesses = value;
				NotifyOfPropertyChange(() => Guesses);
			}
		}

		public string LetterGuess
		{
			get { return _letterGuess; }
			set
			{
				_letterGuess = value;
				NotifyOfPropertyChange(() => LetterGuess);
			}
		}
		public virtual void CheckLetter()
		{
			if (!String.IsNullOrEmpty(LetterGuess))
			{

				_game.EnterGuess(LetterGuess);
				LetterGuess = "";

			}
		}
	}
}
