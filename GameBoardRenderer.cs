using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Radionova.Logic
{
    public static class GameBoardRenderer
    {
        public static TableLayoutPanel Render(Game game, int rows, int cols, Action<TileView> onClick, out List<TileView> tiles)
        {
            var panel = new TableLayoutPanel
            {
                RowCount = rows,
                ColumnCount = cols,
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            for (int i = 0; i < rows; i++)
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));
            for (int j = 0; j < cols; j++)
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / cols));

            tiles = new List<TileView>();
            int index = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var card = game.Cards[index++];
                    var tile = new TileView(card);
                    tile.Picture.Click += (s, e) => onClick(tile);

                    tiles.Add(tile);
                    panel.Controls.Add(tile.Picture, j, i);
                }
            }

            return panel;
        }
    }
}