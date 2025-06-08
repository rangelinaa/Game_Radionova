using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Game_Radionova.Logic
{
    public class TileView
    {
        public Card Card { get; }
        public PictureBox Picture { get; }

        public TileView(Card card)
        {
            Card = card;

            Picture = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
                BorderStyle = BorderStyle.Fixed3D,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Margin = new Padding(2),
                Tag = this
            };

            Update();
        }

        public void Update()
        {
            if (Card.IsMatched || Card.IsFlipped)
            {
                if (File.Exists(Card.ImagePath))
                    Picture.Image = Image.FromFile(Card.ImagePath);
            }
            else
            {
                Picture.Image = null;
            }
        }

        public void Hide()
        {
            Card.Reset();
            Update();
        }

        public void ShowMatch()
        {
            Card.Match();
            Update();
        }

        public void Flip()
        {
            Card.Flip();
            Update();
        }
    }
}