using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.IO;
using static Android.App.ActionBar;

namespace Nauka_angielskiego
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class StatsChooseDictionary : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.StatsChooseDictionary);
            DrawButtonsWithFileNames();
        }
        public void DrawButtonsWithFileNames()
        {
            var currentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            var directoryStatistics = "statistics";
            string filePath = Path.Combine(currentPath, directoryStatistics);
            DirectoryInfo d = new DirectoryInfo(filePath);

            FileInfo[] Files = d.GetFiles("*.csv");
            foreach (FileInfo file in Files)
            {
                Button myButton = new Button(this, null, 0, Resource.Style.buttonTheme);
                myButton.Text = file.Name.Substring(0, file.Name.Length - 4);
                LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.LinearLayoutStatsButtonPanel);
                LayoutParams lp = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                ll.AddView(myButton, lp);
                myButton.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);
                myButton.Click += delegate
                {
                    string temp = filePath;
                    filePath = Path.Combine(temp, file.Name);
                    OpenDictionaryStats(filePath, file.Name.Substring(0, file.Name.Length - 4));
                };
            }
            if (Files.Length == 0)
            {
                Button myButton = new Button(this, null, 0, Resource.Style.buttonTheme);
                myButton.Text = "Brak statystyk do wyświetlenia";
                LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.LinearLayoutStatsButtonPanel);
                LayoutParams lp = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                ll.AddView(myButton, lp);
                myButton.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);
                myButton.Click += delegate
                {
                    Finish();
                };
            }
        }
        public void OpenDictionaryStats(string filePath, string dictionaryName)
        {
                Finish();
                Intent intent = new Intent(this, typeof(StatsChooseStatistic));
                intent.PutExtra("FileName", filePath);
                intent.PutExtra("DictionaryName", dictionaryName);
                StartActivity(intent);
        }
    }
}