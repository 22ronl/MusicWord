using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWord.ViewModels
{
    class FinalPageViewModel : AppScreen
    {
        public void PlayAgain()
        {
            ShellViewModel.Instance.playAgain();
            NextScreen();
        }
        public void EndGame()
        {
            System.Windows.Application.Current.Shutdown();

        }
    }
}
