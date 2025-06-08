using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace Game_Radionova.Logic
{
    public class BackgroundMusicPlayer
    {
        private SoundPlayer player;

        public void Play(string filename)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
                if (File.Exists(path))
                {
                    player = new SoundPlayer(path);
                    player.PlayLooping();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Файл не найден: " + path);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Ошибка воспроизведения: " + ex.Message);
            }
        }

        public void Stop()
        {
            try { player?.Stop(); } catch { }
        }
    }
}