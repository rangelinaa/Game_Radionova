using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Radionova.Logic
{
    public class GameEngine
    {
        public int Rows { get; }
        public int Columns { get; }

        private Random random = new Random();

        public GameEngine(GameSettings settings)
        {
            var size = GetGridSize(settings.Difficulty);
            Rows = size.Item1;
            Columns = size.Item2;
        }

        private Tuple<int, int> GetGridSize(string difficulty)
        {
            if (difficulty == "Средний")
                return Tuple.Create(6, 6);
            else if (difficulty == "Сложный")
                return Tuple.Create(8, 8);
            else
                return Tuple.Create(4, 4);
        }

        public List<Card> GenerateCards()
        {
            int count = Rows * Columns;
            var icons = GetRandomIconPaths(count / 2);

            var paired = icons.SelectMany(path => new[] { path, path })
                              .OrderBy(_ => random.Next())
                              .ToList();

            return paired.Select(path => new Card(path)).ToList();
        }

        private List<string> GetRandomIconPaths(int pairCount)
        {
            string imagesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            var imageFiles = Directory.GetFiles(imagesPath, "*.png").ToList();

            return imageFiles.OrderBy(_ => random.Next()).Take(pairCount).ToList();
        }
    }
}