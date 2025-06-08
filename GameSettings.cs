using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Radionova.Logic
{
    public class GameSettings
    {
        public string Difficulty { get; set; }
        public bool IsMultiplayer { get; set; }

        public GameSettings(string difficulty, bool isMultiplayer)
        {
            Difficulty = difficulty;
            IsMultiplayer = isMultiplayer;
        }
    }
}