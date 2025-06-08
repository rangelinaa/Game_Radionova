using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Radionova.Logic
{
    public class Card
    {
        public string ImagePath { get; }
        public bool IsFlipped { get; private set; }
        public bool IsMatched { get; private set; }

        public Card(string imagePath)
        {
            ImagePath = imagePath;
            IsFlipped = false;
            IsMatched = false;
        }

        public void Flip()
        {
            if (!IsMatched)
                IsFlipped = !IsFlipped;
        }

        public void Match()
        {
            IsMatched = true;
            IsFlipped = true;
        }

        public void Reset()
        {
            IsFlipped = false;
            IsMatched = false;
        }
    }
}