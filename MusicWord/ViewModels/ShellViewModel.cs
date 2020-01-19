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
    public class AppScreen : Screen
    {

        public void NextScreen()
        {
            var screen = ShellViewModel.Instance.getNextScreen();
            if (screen != null)
                ((IShell)this.Parent).ActivateItem(screen());
        }
    }
    
    public interface IShell
    {
        void ActivateItem(Screen screen);
    }
   
    public class ShellViewModel : Conductor<Screen>.Collection.OneActive, IShell
    {
        public delegate Screen GetScreen();
        private static ShellViewModel _instance;
        private Queue<GetScreen> _screens;
        private void createGameScreens()
        {
            List<GetScreen> list = new List<GetScreen> { ()=> new CategoryViewModel() ,
                () => new LevelOneViewModel() , ()=> new ScoreViewModel(), ()=> new FinalScoreViewModel(),
                () => new ScoreTableViewModel(),
                () => new LevelTwoViewModel(Globals.maxClues, Globals.maxLetterGuesses), ()=> new ScoreViewModel(),
                () => new LevelThreeViewModel() , ()=> new ScoreViewModel() };
            _screens = new Queue<GetScreen>(list);
        }
        public  GetScreen getNextScreen()
        {
            if (_screens.Count != 0)
            {
                return _screens.Dequeue();
            }
            return null;
        }
        public ShellViewModel()
        {
            createGameScreens();
            this.ActivateItem(new WelcomeViewModel());
        }
        public static ShellViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ShellViewModel();
                }
                return _instance;
            }
        }
    }
}
