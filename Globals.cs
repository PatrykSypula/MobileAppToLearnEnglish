using Android.App;
using Android.Content;
using Android.Icu.Text;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nauka_angielskiego
{
    static class Globals
    {
        public static string DictionaryPath = 
            Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "dictionaries");
        public static string StatisticsPath = 
            Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "statistics");
        public static string[] GetFiles()
        {
            string[] filesFromAsset = { "Czesci ciala.csv", "Czynnosci.csv", "Jedzenie i picie.csv", "Kolory.csv", "Miejsce zamieszkania.csv", "Natura.csv", "Pogoda.csv", "Przedmioty codziennego uzytku.csv", "Srodki transportu.csv", "Ubrania.csv", "Zawody.csv", "Zwierzeta.csv" };
            return filesFromAsset;
        }
        public static string[] GetFilesPolish()
        {
            string[] filesFromAssetPolish = { "Części ciała.csv", "Czynności.csv", "Jedzenie i picie.csv", "Kolory.csv", "Miejsce zamieszkania.csv", "Natura.csv", "Pogoda.csv", "Przedmioty codziennego użytku.csv", "Środki transportu.csv", "Ubrania.csv", "Zawody.csv", "Zwięrzeta.csv" };
            return filesFromAssetPolish;

        }
        public static List<string> GetListOfFileNames()
        {
            DirectoryInfo d = new DirectoryInfo(Globals.DictionaryPath);
            FileInfo[] Files = d.GetFiles("*.csv");
            string[] filesFromAssetPolish = GetFilesPolish();
            List<string> strings = new List<string>();

            foreach (string file in filesFromAssetPolish)
            {
                strings.Add(file);
            }
            foreach (FileInfo filee in Files)
            {
                strings.Add(filee.Name);
            }
            return strings;
        }
        public static void ShortToast(string Message)
        {
            Toast.MakeText(Android.App.Application.Context, Message, ToastLength.Short).Show();
        }
    }
}