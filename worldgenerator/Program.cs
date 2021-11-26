using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using SimplexNoise;

namespace worldgenerator
{
    class Program
    {
        static float[,] landscape;
        static float[,] biomes;

        enum biomeList { PLAINS, FOREST, JUNGLE, SAVANNAH, DESERT };
        static biomeList GetBiome(int x, int y)
        {
            int level = Convert.ToInt32(biomes[x, y]);
            if (level < 55)
            {
                return biomeList.PLAINS;
            }
            else if (level >= 55 && level < 110)
            {
                return biomeList.FOREST;
            }
            else if (level >= 110 && level < 165)
            {
                return biomeList.JUNGLE;
            }
            else if (level >= 165 && level < 220)
            {
                return biomeList.SAVANNAH;
            }
            else
            {
                return biomeList.DESERT;
            }
        }
        static Pen GetBiomePen(biomeList biome)
        {
            switch (biome)
            {
                case (biomeList.PLAINS): { return Pens.Green; }
                case (biomeList.FOREST): { return Pens.ForestGreen; }
                case (biomeList.JUNGLE): { return Pens.Lime; }
                case (biomeList.SAVANNAH): { return Pens.Olive; }
                case (biomeList.DESERT): { return Pens.NavajoWhite; }
                default: { return null; }
            }
        }

        static Pen GetPen(int x, int y, int level)
        {
            if (level < 30)
            {
                return Pens.DarkBlue;
            }
            else if (level >= 30 && level < 90)
            {
                return Pens.Blue;
            }
            else if (level >= 90 && level < 105)
            {
                return Pens.Yellow;
            }
            else if (level >= 105 && level < 200)
            {
                return GetBiomePen(GetBiome(x, y));
            }
            else if (level >= 200 && level < 235)
            {
                return Pens.Gray;
            }
            else
            {
                return Pens.White;
            } 
        }
        static void Main(string[] args)
        {
            Console.WriteLine("С:\\Users\\" + Environment.UserName + "\\Pictures\\generated.png");
            Console.Write("Enter seed (blank for random) > ");
            string seed = Console.ReadLine();
            Console.Write("Enter height (1024 - default) > ");
            string stringheight = Console.ReadLine();
            int height = 1024;
            if (stringheight != "") { height = Convert.ToInt32(stringheight); }
            Console.Write("Enter width (1024 - default) > ");
            string stringwidth = Console.ReadLine();
            int width = 1024;
            if (stringheight != "") { height = Convert.ToInt32(stringheight); }

            if (seed.Trim() != "") { SimplexNoise.Noise.Seed = Convert.ToInt32(seed); }
            landscape = Noise.Calc2D(width, height, 0.001f);
            biomes = Noise.Calc2D(width, height, 0.01f);

            Bitmap image = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image);

            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    graphics.DrawRectangle(GetPen(x, y, (int)landscape[x, y]), x, y, 1, 1);
                }
            }

            bool v = Directory.Exists("D:\\tempforgenerator");
            if (v)
            {
                Console.WriteLine("directory exist, saving...");
            }
            else if (!v)
            {
                Console.WriteLine("Can't find directory!");
                Console.WriteLine("Creating new folder...");
                Directory.CreateDirectory("D:\\tempforgenerator\\");
            }

            image.Save("D:\\tempforgenerator\\generated.png", ImageFormat.Png);
            Console.WriteLine("Generated image and saved into generated.png");
            Console.ReadLine();
        }
    }
}
