using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWord.ViewModels
{
    /// <summary>
    ///  Class <c>InstructionsGuideViewModel</c> responsible for desplaying the game instructions.
    /// </summary>
    class InstructionsGuideViewModel : AppScreen
    {
        public void NextBottum()
        {
            NextScreen();
        }
    }
}
