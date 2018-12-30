using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanalista
{
    class Finder
    {
        //static HashSet<string> AnsList = new HashSet<string>(); //You can use "HashSet" if you want only words. Much better.
        public static async Task StartAsync(Node rootnode, char[,] Quiz) {
            List<WordInfo> AnsList=new List<WordInfo>();
            for (int i = 0; i < Generate.square; i++)
                for (int j = 0; j < Generate.square; j++)
                    AnsList.AddRange(await GoSearch(rootnode, Quiz, i, j, new List<WordInfo>()));
            AnsList=AnsList.GroupBy(x => x.Word).Select(x=>x.First()).Where(x => !String.IsNullOrEmpty(x.Word)).Distinct().ToList();//Remove Duplication. Recommended to make outside of the Multitasks.
            Console.WriteLine(AnsList.Count);
            foreach (var s in AnsList)
            {
                //Console.WriteLine("Found: " + s);
                await Console.Out.WriteAsync("Found: " + s.Word);
                if (s.Order != null)
                {
                    foreach (int[] arr in s.Order)
                        await Console.Out.WriteAsync("[" + arr[0] + ", " + arr[1] + "]");
                }
                await Console.Out.WriteAsync("\n");
            }
        }
        public static async Task<List<WordInfo>> GoSearch(Node wnode, char[,] cList, int x, int y, List<WordInfo> AnsList)
        {
            cList = cList.Clone() as char[,];
            //You know, cause' whole array size<10, we'll use array[] with 10*x+y
            char c = cList[x, y];
            if (!wnode.Children.ContainsKey(c) || c == '\0') return AnsList;
            cList[x, y] = '\0';//Marked as "already used"
            wnode = wnode.Children[c];
            try
            {
                if (wnode.IsWord)// && !AnsList.Any(w => w.Word == wnode.Word))
                {
                    AnsList.Add(new WordInfo(wnode.Word, Counter(cList)));
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Ooops Exception occured X( :"+ex.GetType()+" with "+wnode.Word);
                //Ignore it.
            }
            List<Task> tasks = new List<Task>();
            if (x < 3)
            {
                tasks.Add(Task.Run(() => GoSearch(wnode, cList, x + 1, y, AnsList)));
                if (y < 3) tasks.Add(Task.Run(() => GoSearch(wnode, cList, x + 1, y + 1, AnsList)));
                if (y != 0) tasks.Add(Task.Run(() => GoSearch(wnode, cList, x + 1, y - 1, AnsList)));
            }
            if (y < 3)
            {
                tasks.Add(Task.Run(() => GoSearch(wnode, cList, x, y + 1, AnsList)));
                if (x != 0) tasks.Add(Task.Run(() => GoSearch(wnode, cList, x - 1, y + 1, AnsList)));
            }
            if (x != 0)
            {
                tasks.Add(Task.Run(() => GoSearch(wnode, cList, x - 1, y, AnsList)));
                if (y != 0) tasks.Add(Task.Run(() => GoSearch(wnode, cList, x - 1, y - 1, AnsList)));
            }
            if (y != 0)
            {
                tasks.Add(Task.Run(() => GoSearch(wnode, cList, x, y - 1, AnsList)));
            }
            // End
            await Task.WhenAll(tasks);
            return AnsList;
        }
        public static List<int[]> Counter(char[,] cList) {
            List<int[]> xy = new List<int[]>();
            for (int i = 0; i < Generate.square; i++)
            {
                for (int j = 0; j < Generate.square; j++)
                {
                    if(cList[i,j]=='\0')
                    xy.Add(new int[2] { i, j });
                }
            }
            return xy;
        }
    }
    struct WordInfo {
        public string Word;
        public List<int[]> Order;
        public WordInfo(string s, List<int[]> o){
            Word = s;
            Order = o;
        }
    }
}