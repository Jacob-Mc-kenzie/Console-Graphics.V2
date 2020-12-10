using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComapactGraphicsV2;

namespace TestApp
{
    public class Life
    {
        int[][] cells;
        int[][] Ncells;
        static Random rng = new Random();
        int width, height;
        int timeStep;
        public Life(int w, int h, int timestep, int rpercent)
        {

            width = w;
            height = h;
            timeStep = timestep;
            cells = new int[h][];
            Ncells = new int[h][];
            for (int i = 0; i < h; i++)
            {
                cells[i] = new int[w];
                Ncells[i] = new int[w];
                for (int j = 0; j < w; j++)
                {
                    bool k = rng.Next(0, 100) < rpercent;
                    cells[i][j] = k ? 1 : 0;
                    Ncells[i][j] = k ? 1 : 0;
                }
            }

        }

        public void Step(PixelGrid target)
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    int C = countSourounds(i, j);
                    if (C == 3 || C == 2 && (Ncells[i][j] == 1))
                        cells[i][j] = 1;
                    else
                        cells[i][j] = (!new int[] { 0, 4 }.Contains(Ncells[i][j])) ? Ncells[i][j] + 1 : 0;
                    if(cells[i][j] == 1)
                        target.DrawPixel(i, j, ConsoleColor.White);
                }
            for (int i = 0; i < height; i++)
                Array.Copy(cells[i], 0, Ncells[i], 0, width);
            //System.Threading.Thread.Sleep(timeStep);
        }

        private int countSourounds(int i, int j)
        {
            int count = 0;
            int w = width - 1;
            int h = height - 1;
            for (int Y = i - 1; Y <= i + 1; Y++)
                for (int X = j - 1; X <= j + 1; X++)
                    if (i != Y || X != j)
                        if (Ncells[Y < 0 ? h : Y > h ? 0 : Y][X < 0 ? w : X > w ? 0 : X] == 1)
                            count++;
            return count;
        }
    }
}

