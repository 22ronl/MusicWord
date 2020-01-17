using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MusicWord.Models;
namespace MusicWord.ViewModels
{
    class BaseLevelViewModel : AppScreen
    {
		protected int _score;
		protected string _guessWord;
		protected string _time;
		protected string _hiddenWord;
		protected TimeModel _timer;
		private BaseLevelModel _game;
		protected CluesModel _cluesGenrator;
		protected string _clue;
		public BaseLevelViewModel()
		{
		}
		protected void baseBuilder(BaseLevelModel game, int score, string word)
		{
			Score = score;
			_game = game;
			Category = Globals.categoryText + PlayerModel.Instance.Category;
			PlayerModel.Instance.lastWord = word;
		}


		protected void _game_GameOver(object sender, EventArgs e)
		{
			NextScreen();
		}

		public virtual void _game_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Globals.hiddenWord)
			{
				HiddenWord = _game.HiddenWord;
			}

			else if (e.PropertyName == Globals.score)
			{
				Score = _game.Score;
			}
		}

		protected void _timer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			PropertyInfo property = sender.GetType().GetProperty(e.PropertyName);
			int secondes = (int)property.GetValue(sender, null);
			TimeSpan t = TimeSpan.FromSeconds(secondes);
			Time = t.ToString();
		}
		public string Time
		{
			get { return _time; }
			set
			{
				_time = value;
				NotifyOfPropertyChange(() => Time);
			}
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

		public string Clue
		{
			get { return _clue; }
			set 
			{ 
				_clue = value;
				NotifyOfPropertyChange(() => Clue);
			}
		}

		public virtual void GetClue()
		{
			string clue = _cluesGenrator.getClue();
			Clue = clue;
		}

		public void CheckWord()
		{
			if (!String.IsNullOrEmpty(GuessWord))
			{
				
				_game.EnterWord(GuessWord);
				GuessWord = "";
			}
		}


		public string GuessWord
		{
			get { return _guessWord; }
			set
			{
				_guessWord = value;
				NotifyOfPropertyChange(() => GuessWord);
			}
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

		public string Category { get; set; }

	}
}

