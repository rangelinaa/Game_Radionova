using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Radionova.Logic
{
    public static class GameResultDisplayer
    {
        public static void Show(Game game, Player player1, Player player2)
        {
            string message;
            if (game.Players.Count == 2)
            {
                if (player1.Score > player2.Score)
                    message = $"Победа игрока 1!\nСчёт: {player1.Score} - {player2.Score}";
                else if (player2.Score > player1.Score)
                    message = $"Победа игрока 2!\nСчёт: {player1.Score} - {player2.Score}";
                else
                    message = $"Ничья!\nСчёт: {player1.Score} - {player2.Score}";
            }
            else
            {
                message = "Вы выиграли! Все карточки открыты";
            }

            MessageBox.Show(message, "Результат");
        }
    }
}