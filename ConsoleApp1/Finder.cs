﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sanalista
{
    class Finder
    {
        private Node RootNode { get; set; }
        private char[,] Quiz { get; set; }
        ConcurrentDictionary<int[][], string> AllResult = new ConcurrentDictionary<int[][], string>();
        public Finder(Node rootnode, char[,] quiz) {
            RootNode = rootnode;
            Quiz = quiz;
            StartAsync().Wait();
        }
        private async Task StartAsync() {
            //Not dictionary: Same word can have multiple solution.
            Task[] tasks = new Task[Generate.square * Generate.square];
            for (int i = 0; i < Generate.square; i++)
            {
                for (int j = 0; j < Generate.square; j++)
                {
                    tasks[Generate.square * i + j] = WordTask(i, j);
                }
            }
            Console.WriteLine();
            await Task.WhenAll(tasks);
            foreach (var s in AllResult)
            {
                //Console.WriteLine("Found: " + s.Value);
                await Console.Out.WriteAsync("Found: " + s.Value);
                if (s.Key != null)
                {
                    foreach (int[] arr in s.Key)
                        await Console.Out.WriteAsync("[" + arr[0] + ", " + arr[1] + "]");
                }
                await Console.Out.WriteAsync("\n");
            }
        }
        private async Task<ConcurrentDictionary<int[][], string>> GoSearch(Node wnode, char[,] cList, int x, int y, ConcurrentDictionary<int[][], string> AnsList)
        {
            cList = cList.Clone() as char[,];
            //You know, cause' whole array size<10, we'll use array[] with 10*x+y
            char c = cList[x, y];
            if (!wnode.Children.ContainsKey(c) || c == '\0') return AnsList;
            cList[x, y] = '\0';//Marked as "already used"
            wnode = wnode.Children[c];
            if (wnode.IsWord)// && !AnsList.Any(w => w.Word == wnode.Word))
            {
                AnsList.TryAdd(Counter(cList).ToArray(), wnode.Word);
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
        private List<int[]> Counter(char[,] cList) {
            List<int[]> xy = new List<int[]>();
            for (int i = 0; i < Generate.square; i++)
            {
                for (int j = 0; j < Generate.square; j++)
                {
                    if (cList[i, j] == '\0')
                        xy.Add(new int[2] { i, j });
                }
            }
            return xy;
        }
        private async Task<bool> WordTask(int x, int y)
        {
            var result = await GoSearch(RootNode, Quiz, x, y, new ConcurrentDictionary<int[][], string>());
            bool res=true;
            foreach (var pair in result)
            {
                 res&=AllResult.TryAdd(pair.Key, pair.Value);
            }
            return res;
        }
    }
}