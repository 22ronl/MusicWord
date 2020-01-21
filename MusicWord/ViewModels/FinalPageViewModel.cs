using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWord.ViewModels
{
    /// <summary>
    ///  Class <c>FinalPageViewModel</c> responsible on executing two bottums in FinalPageView.
    /// </summary>
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
