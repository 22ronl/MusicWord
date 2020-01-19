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
        private List<GetScreen> _levels;
        private void createGameScreens()
        {
            _levels = new List<GetScreen> { ()=> new CategoryViewModel() , ()=> new InstructionsGuideViewModel(),
                () => new LevelOneViewModel() , ()=> new ScoreViewModel(), 
                () => new LevelTwoViewModel(Globals.maxClues, Globals.maxLetterGuesses), ()=> new ScoreViewModel(),
                () => new LevelThreeViewModel() , ()=> new ScoreViewModel(),
                ()=> new FinalScoreViewModel(),
                () => new ScoreTableViewModel(),};
            _screens = new Queue<GetScreen>(_levels);
        }
        public  GetScreen getNextScreen()
        {
            if (_screens.Count != 0)
            {
                return _screens.Dequeue();
            }
            return null;
        }
        public void playAgain()
        {
            _screens = new Queue<GetScreen>(_levels);
            PlayerModel.Instance.Score = 0;
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
