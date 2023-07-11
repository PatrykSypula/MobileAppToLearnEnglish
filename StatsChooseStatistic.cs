using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using static Android.App.ActionBar;
using static Android.Provider.UserDictionary;

namespace Nauka_angielskiego
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class StatsChooseStatistic : AppCompatActivity
    {
        string filePath;
        string fileName;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.StatsChooseStatistic);
            filePath = Intent.GetStringExtra("FileName");
            fileName = Intent.GetStringExtra("DictionaryName");
            DrawButtonsWithFileNames();
            
        }
        public void DrawButtonsWithFileNames()
        {
            StreamReader reader = new StreamReader(filePath);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string selectedDifficulty = "";
                string[] values = line.Split(";");
                switch (values[0])
                {
                    case "1":
                        selectedDifficulty = "Ucz się ";
                        break;
                    case "2":
                        selectedDifficulty = "Sprawdź się ";
                        break;
                    case "3":
                        selectedDifficulty = "Egzamin ";
                        break;
                }
                string totalShown = values[1];
                string goodAnswer = values[2];
                string date = values[3];
                Button myButton = new Button(this, null, 0, Resource.Style.buttonTheme);
                myButton.Text = selectedDifficulty + Math.Round((double.Parse(goodAnswer)/ double.Parse(totalShown)) * 100, 2).ToString() + "%"; ;
                LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.LinearLayoutStatsPanel);
                LayoutParams lp = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                ll.AddView(myButton, lp);
                myButton.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);
                myButton.Click += delegate
                {
                    Finish();
                    Intent intent = new Intent(this, typeof(Statistics));
                    intent.PutExtra("answersGood", goodAnswer);
                    intent.PutExtra("answersShown", totalShown);
                    intent.PutExtra("selectedDifficulty", selectedDifficulty);
                    intent.PutExtra("date", date);
                    intent.PutExtra("fileName", fileName);
                    StartActivity(intent);
                };
            }
            reader.Close();
        }
        public void OpenStatistic(string fileName)
        {
            Finish();
            Intent intent = new Intent(this, typeof(Session));
            intent.PutExtra("FileName", fileName);
            StartActivity(intent);
        }
    }
}