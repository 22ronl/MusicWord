using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MusicWord.Models;
namespace MusicWord.ViewModels
{
    public class WelcomeViewModel : AppScreen
	{
		private string _name ;
		
		public string PlayerName
		{
			get { return _name; }
			set { _name = value; }
		}
		public void Start()
		{
			///<summary>
			///
			///</summary>
			if (!string.IsNullOrEmpty(PlayerName))
			{
                PlayerModel newPlayer = PlayerModel.Instance;
                newPlayer.Name = PlayerName;
                NextScreen();
			}
			else
			{
				NoNameError = Globals.errorMessage;
			}
		}
		private string _noNameError;

		public string NoNameError
		{
			get { return _noNameError; }
			set 
			{
				_noNameError = value;
				NotifyOfPropertyChange(() => NoNameError);
			}
		}



	}

}
