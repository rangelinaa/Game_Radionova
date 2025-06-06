using Game_Radionova.Logic;
using System;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace Game_Radionova
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string level = comboBox1.SelectedItem as string;
            if (string.IsNullOrEmpty(level))
            {
                MessageBox.Show("Выберите сложность.");
                return;
            }

            bool isMultiplayer = radioButtonTwoPlayer.Checked;
            var settings = new GameSettings(level, isMultiplayer);

            var gameForm = new GameForm(settings);
            gameForm.Show();
            Hide();
        }

        private void radioButtonSinglePlayer_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButtonTwoPlayer_CheckedChanged(object sender, EventArgs e)
        {
            
        }
    }
}