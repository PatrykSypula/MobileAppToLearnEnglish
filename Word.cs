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
    internal class Word
    {
        public string polishWord { get; set; }
        public string englishWord { get; set; }

        public Word(string polishWord, string englishWord)
        {
            this.polishWord = polishWord;
            this.englishWord = englishWord;
        }
        public override string ToString()
        {
            return polishWord + ", " + englishWord;
        }
    }
}