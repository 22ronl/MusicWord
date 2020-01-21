using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWord.Models
{
    static class Globals
    {
        /// <summary>Class <c>Globals</c>
        /// Saving all global varibles needed for the game
        /// </summary>
        public static string errorMessage = "Error: enter your name";
        public static int levelTime = 100;
        public static double hidddenPercentage = 0.40;
        public static string connectionString = "SERVER = localhost; DATABASE=musicword; UID= root; PASSWORD=matan1623724";
        public static string hiddenWord = "HiddenWord";
        public static string guesses = "Guesses";
        public static string score = "Score";
        public static string noLetters = "No letter guesses left";
        public static string noClues = "No clues left";
        public static string hiddenWordText = "The hidden word was: ";
        public static string categoryText = "Category: ";
        public static int maxCluesBase = 10;
        public static int letterScore = 50;
        public static int wordScore = 200;
        public static int secondsScore = 10;
        public static int maxClues = 5;
        public static int maxLetterGuesses = 5;
    }
}
