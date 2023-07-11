using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nauka_angielskiego
{
    internal class CardStack
    {
        public int index { get; set; }
        public List<Word> words = new List<Word>();
        public int current { get; set; }
        public void AddWord(Word _word)
        {
            words.Add(_word);
        }
        public Word PrintWord()
        {
            Random random = new Random();
            index = random.Next(words.Count);
            if (words.Count != 0)
            {
                if (words.Count != 1)
                {
                    while (current == index)
                    {
                        index = random.Next(words.Count);
                    }
                }
                return words[index];
            }
            return null;
        }
        public void RemoveWord(int number)
        {
            words.RemoveAt(number);
        }
        public Word RollWord()
        {
            Random random = new Random();
            if (words.Count != 1)
            {
                current = index;
                while (current == index)
                {
                    index = random.Next(words.Count);
                }
            }
            index = random.Next(words.Count);
            return words[index];
        }
        public List<Word> GetList()
        {
            return words;
        }
    }
}