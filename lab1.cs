using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ColorPalette
{
    class Program
    {
        static void Main()
        {
            string text = File.ReadAllText("Aeroport.txt");
            var found = FindColors(text);

            Console.WriteLine($"Найдено цветов: {found.Count}\n");

            foreach (var f in found)
                Console.WriteLine(f.Word);

            SavePalette(found.Select(f => f.Color).ToList(), "ColorPalette.png");
            Console.WriteLine("\nГотово: ColorPalette.png");
        }

        static Dictionary<string, Color> colorMap = new Dictionary<string, Color>(StringComparer.OrdinalIgnoreCase)
        {
            { "красн", Color.Red },
            { "алый", Color.Crimson },
            { "багр", Color.DarkRed },
            { "зелен", Color.Green },
            { "изумруд", Color.MediumSeaGreen },
            { "син", Color.Blue },
            { "голуб", Color.LightBlue },
            { "лазур", Color.LightSkyBlue },
            { "желт", Color.Yellow },
            { "золот", Color.Gold },
            { "бел", Color.White },
            { "черн", Color.Black },
            { "сер", Color.Gray },
            { "фиолетов", Color.Purple },
            { "оранжев", Color.Orange },
            { "коричнев", Color.Brown },
            { "розов", Color.Pink },
            { "бирюз", Color.Turquoise }
        };

        static List<(string Word, Color Color)> FindColors(string text)
        {
            var result = new List<(string, Color)>();
            var words = Regex.Matches(text, @"\b[\p{IsCyrillic}]+\b");

            foreach (Match m in words)
            {
                string word = m.Value.ToLowerInvariant();
                foreach (var pair in colorMap)
                {
                    if (word.StartsWith(pair.Key))
                    {
                        result.Add((m.Value, pair.Value));
                        break;
                    }
                }
            }
            return result;
        }

        static void SavePalette(List<Color> colors, string path)
        {
            int size = 30, pad = 2, perRow = 20;
            int rows = (colors.Count + perRow - 1) / perRow;

            using var bmp = new Bitmap(perRow * (size + pad) + pad, rows * (size + pad) + pad);
            using var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            for (int i = 0; i < colors.Count; i++)
            {
                int x = (i % perRow) * (size + pad) + pad;
                int y = (i / perRow) * (size + pad) + pad;
                using var brush = new SolidBrush(colors[i]);
                g.FillRectangle(brush, x, y, size, size);
            }
            bmp.Save(path, ImageFormat.Png);
        }
    }
}
