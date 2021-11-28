using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using SimplexNoise;

namespace worldgenerator
{
    class Program
    {
        static float[,] landscape;
        

        static Pen GetPen(int x, int y, int level, int temp)
        {
            if (0 < level && level <= 40)
            {
                if (temp <= 128) { return Pens.SkyBlue; }
                else if (128 < temp && temp <= 255) { return Pens.Navy; }
                else { return Pens.Black; }
            }
            else if (40 < level && level <= 100)
            {
                if (temp <= 128) { return Pens.LightBlue; }
                else if (128 < temp && temp <= 255) { return Pens.Blue; }
                else { return Pens.Black; }
            }
            else if (100 < level && level <= 220)
            {
                if (temp <= 128) { return Pens.Snow; }
                else if (128 < temp && temp <= 230) { return Pens.ForestGreen; }
                else if (230 < temp && temp <= 255) { return Pens.Yellow; }
                else { return Pens.Black; }
            }
            else if (220 < level && level <= 255)
            {
                if (temp <= 25) { return Pens.Snow; }
                else if (25 < temp && temp <= 128) { return Pens.Gray; }
                else if (128 < temp && temp <= 190) { return Pens.DarkGreen; }
                else if (190 < temp && temp <= 255) { return Pens.ForestGreen; }
                //else if (200 < temp && temp <= 255) { return Pens.Yellow; }
                else { return Pens.Black; }
            }

            else { return Pens.Black; }


            /*if (25 > temp) { return Pens.Snow; }
            else if (25 <= temp && temp < 50) { return Pens.LightBlue; }
            else if (50 <= temp && temp < 75) { return Pens.LightSkyBlue; }
            else if (75 <= temp && temp < 100) { return Pens.SkyBlue; }
            else if (100 <= temp && temp < 125) { return Pens.DeepSkyBlue; }
            else if (125 <= temp && temp < 150) { return Pens.DarkBlue; }
            else { return Pens.Red; }*/

        }
        static void Main(string[] args)
        {
            Console.Write("Enter landscape seed (blank for random) > ");
            string landscapeseed = Console.ReadLine().Trim();
            Console.Write("Enter temperature seed (blank for random) > ");
            string temperatureseed = Console.ReadLine().Trim();
            Console.Write("Enter height (1024 - default) > ");
            string stringheight = Console.ReadLine();
            int height = 1024;
            if (stringheight != "") { height = Convert.ToInt32(stringheight); }
            Console.Write("Enter width (1024 - default) > ");
            string stringwidth = Console.ReadLine();
            int width = 1024;
            if (stringwidth != "") { width = Convert.ToInt32(stringwidth); }

            float[,] temperature = new float[height, width];


            if (landscapeseed != "") { Noise.Seed = Convert.ToInt32(landscapeseed); }
            landscape = Noise.Calc2D(width, height, 10.0f/(height+width));
            if (temperatureseed != "") { Noise.Seed = Convert.ToInt32(temperatureseed); }
            temperature = Noise.Calc2D(width, height, 9.0f / (height + width));
            for (int y = 0; y < height; y++)
            {
                float polusfactor = ((float)Math.Abs(Math.Sin(y / (height/10f) / Math.PI)) * 255.0f - 128f);
                float landscapefactor = 0.0f;
                for (int x = 0; x < width; x++)
                {
                    temperature[x, y] += polusfactor;
                    if (landscape[x, y] >= 220 )
                    {
                    landscapefactor = (220.0f - landscape[x, y])* 6.0f;
                        temperature[x, y] += landscapefactor;
                    }
                    if (temperature[x, y] > 255) { temperature[x, y] = 255.0f; }
                    else if (temperature[x, y] < 0) { temperature[x, y] = 0.0f; }

                }
                Console.Write("\rFor y:{0} have polusfactor: {1} , Percent: " + ((int)(y * 100 / (height))) + " % ", y, polusfactor);
            }
            Console.WriteLine("\nPolus settings configured");
            Bitmap image = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image);
            Console.WriteLine("Painting image, please wait, it can took a few minutes");
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    graphics.DrawRectangle(GetPen(x, y, (int)landscape[x, y], (int)temperature[x, y]), x, y, 1, 1);
                }
                Console.Write("\rPercent: " + ((int)(y * 100 / height)) + "%");
            }

            if (Directory.Exists("D:\\tempforgenerator"))
            {
                Console.WriteLine("\ndirectory exist, saving...");
            }
            else if (!Directory.Exists("D:\\tempforgenerator"))
            {
                Console.WriteLine("\nCan't find directory!");
                Console.WriteLine("Creating new folder...");
                Directory.CreateDirectory("D:\\tempforgenerator\\");
            }

            image.Save("D:\\tempforgenerator\\generated.png", ImageFormat.Png);
            Console.WriteLine("Generated image and saved into generated.png");
        }
    }
}
