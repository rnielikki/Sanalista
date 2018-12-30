using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanalista
{
    class Generate
    {
        public const int square= 4;
        public static char[,] MakeQuiz() {
            string strArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖ";
            char[,] grid;
            bool F;
            do {
                grid = new char[square, square];
                F = FillWord(grid);Console.WriteLine("Trying");
            } while (!F); //If the way is closed... not good
            Random rand = new Random();
            for (int i = 0; i < square; i++) {
                for (int j = 0; j < square; j++) {
                    if (grid[i,j]!= '\0') continue;
                    grid[i, j] = strArray[rand.Next() % strArray.Length];
                }
            }

            return grid;
        }
        public static bool FillWord(char[,] Quiz)
        {
            Random rand = new Random();
            string randword=Nodes.LongWords.ElementAt(rand.Next(Nodes.LongWords.Count));
            Console.WriteLine(randword);
            int[] Way;
            int x =rand.Next(4), y = rand.Next(4);//Set initial position
            foreach (char c in randword)
            {
                Way = FindWays(x, y,Quiz);
                if (Way[0] == -1) return false;
                Quiz[Way[0], Way[1]] = c;
                x = Way[0]; y = Way[1];
            }
            return true;
        }
        public static void PrintQuiz(char[,] Quiz){
            for(int i=0;i<square;i++){
                for(int j = 0; j < square; j++) {
                    Console.Write(Quiz[i,j]);
                }
                Console.Write("\n");
            }
        }
        public static int[] FindWays(int x, int y, char[,] Map) {
            List<int[]> Ways = new List<int[]>();
            Random rand = new Random();
            if (x < 3)
            {
                if (Map[x + 1, y] == '\0') Ways.Add(new int[2] { x + 1, y });
                if (y < 3 && Map[x+1,y+1]=='\0') Ways.Add(new int[2] { x + 1, y + 1 });
                if (y != 0 && Map[x + 1, y - 1] == '\0') Ways.Add(new int[2] { x + 1, y - 1 });
            }
            if (y < 3)
            {
                if (Map[x, y + 1] == '\0') Ways.Add(new int[2] { x, y + 1 });
                if (x != 0 && Map[x - 1, y + 1] == '\0') Ways.Add(new int[2] { x - 1, y + 1 });
            }
            if (x != 0)
            {
                if (Map[x - 1, y] == '\0') Ways.Add(new int[2] { x - 1, y });
                if (y != 0 && Map[x - 1, y - 1] == '\0') Ways.Add(new int[2] { x - 1, y - 1 });
            }
            if (y != 0)
            {
                if (Map[x, y - 1] == '\0') Ways.Add(new int[2] { x, y - 1 });
            }
            if (Ways.Count == 0) return new int[2] { -1 , -1};
            return Ways[rand.Next(Ways.Count)];
        }
    }
}
