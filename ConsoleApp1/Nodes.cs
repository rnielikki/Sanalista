using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Sanalista
{
    class Nodes
    {
        public static HashSet<string> LongWords = new HashSet<string>();
        public static Node Maker() {
            IEnumerable<string> words = File.ReadLines(@"..\..\kotus-sanalista_regex_V2.txt");
            Node rootnode =MakeData(words);
            return rootnode;
            //read more about trie algorithm
            //https://en.wikipedia.org/wiki/Trie
        }
        static Node MakeData(IEnumerable<string> wordList){
            Node root = new Node("");
            Node node;
            foreach (string str in wordList) {
                node = root;
                foreach (char c in str) {
                    if (!node.Children.ContainsKey(c)) {
                        node.Children.Add(c, new Node(node.Word+c));
                    }
                    node = node.Children[c];
                }
                node.IsWord = true;
                if (node.Word.Length > 6) LongWords.Add(node.Word);
            }
            return root;
        }
    }
    class Node {
        public string Word;
        public bool IsWord;
        public Dictionary<char,Node> Children;
        public Node(string word) {
            Word = word;
            Children = new Dictionary<char, Node>();
        }
    }
}