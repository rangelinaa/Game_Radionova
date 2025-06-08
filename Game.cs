using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Radionova.Logic
{
    public class Game
    {
        public List<Player> Players { get; }
        public int CurrentPlayerIndex { get; private set; }
        public List<Card> Cards { get; }

        private Card firstSelected;
        private Card secondSelected;

        public Card FirstCard => firstSelected;
        public Card SecondCard => secondSelected;

        public Player CurrentPlayer => Players[CurrentPlayerIndex];

        public bool BothCardsSelected() => firstSelected != null && secondSelected != null;

        public Game(List<Card> cards, List<Player> players)
        {
            Cards = cards;
            Players = players;
            CurrentPlayerIndex = 0;
        }

        public bool SelectCard(Card card)
        {
            if (card.IsFlipped || card.IsMatched)
                return false;

            if (firstSelected == null)
            {
                firstSelected = card;
                card.Flip();
                return true;
            }

            if (secondSelected == null && card != firstSelected)
            {
                secondSelected = card;
                card.Flip();
                return true;
            }

            return false;
        }

        public bool CheckMatch()
        {
            if (firstSelected == null || secondSelected == null)
                return false;

            if (firstSelected.ImagePath == secondSelected.ImagePath)
            {
                firstSelected.Match();
                secondSelected.Match();
                Players[CurrentPlayerIndex].AddPoint();

                return true;
            }

            return false;
        }

        public void NextTurn()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
        }

        public void ResetTurn()
        {
            firstSelected = null;
            secondSelected = null;
        }

        public bool IsGameFinished() => Cards.All(c => c.IsMatched);
    }
}
