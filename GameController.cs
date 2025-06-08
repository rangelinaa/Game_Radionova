using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Radionova.Logic
{
    public class GameController
    {
        private readonly Game game;

        public Card FirstCard => game.FirstCard;
        public Card SecondCard => game.SecondCard;
        public Player CurrentPlayer => game.CurrentPlayer;

        public event Action<Card, Card, bool> OnMatchChecked;
        public event Action<Player> OnTurnChanged;
        public event Action<Player, Player> OnGameFinished;

        public GameController(Game game)
        {
            this.game = game;
        }

        public bool SelectCard(Card card)
        {
            if (!game.SelectCard(card)) return false;

            if (game.BothCardsSelected())
            {
                bool match = game.CheckMatch();
                OnMatchChecked?.Invoke(FirstCard, SecondCard, match);

                if (game.IsGameFinished())
                {
                    var p1 = game.Players[0];
                    var p2 = game.Players.Count > 1 ? game.Players[1] : null;
                    OnGameFinished?.Invoke(p1, p2);
                }

                if (!match)
                {
                    game.NextTurn();
                    OnTurnChanged?.Invoke(CurrentPlayer);
                }
            }

            return true;
        }


        public void ResetTurn()
        {
            game.ResetTurn();
        }
    }
}