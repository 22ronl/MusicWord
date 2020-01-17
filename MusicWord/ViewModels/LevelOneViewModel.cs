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

		protected string _letterGuess;
		protected string _guesses;
		protected LevelOneModel _game;
		public LevelOneViewModel()
		{
			ICategory category = SQLServerModel.getWord(PlayerModel.Instance.Category);
			_cluesGenrator = new CluesModel(category);
			string word = category.Name;
			_timer = new TimeModel(100);
			_timer.PropertyChanged += _timer_PropertyChanged;
			int score = PlayerModel.Instance.Score;
			_game = new LevelOneModel(word, _timer, Globals.hidddenPercentage,score);
			_game.PropertyChanged += _game_PropertyChanged;
			_game.GameOver += _game_GameOver;
			_game.start();
			baseBuilder(_game, score,word);
		}

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
