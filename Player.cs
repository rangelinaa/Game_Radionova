using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Game_Radionova.Logic
{
    public class Player
    {
        public string Name { get; }
        public int Score { get; private set; }

        public Player(string name)
        {
            Name = name;
            Score = 0;
        }

        public void AddPoint()
        {
            Score++;
        }

        public void ResetScore()
        {
            Score = 0;
        }
    }
}