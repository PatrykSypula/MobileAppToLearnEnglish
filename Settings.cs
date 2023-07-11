using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.IO;
using System.Runtime.Remoting.Channels;

namespace Nauka_angielskiego
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class Settings : AppCompatActivity
    {
        Switch switchStats;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings);

            RadioButton radioButton0 = FindViewById<RadioButton>(Resource.Id.radioButton0);
            RadioButton radioButton1 = FindViewById<RadioButton>(Resource.Id.radioButton1);
            string whichToSelect = ReadOrderFromUserSettings();

            switchStats = FindViewById<Switch>(Resource.Id.switchStats);

            switchStats.Checked = ReadIfStatsShouldBeShownFromUserSettings();

            if (whichToSelect == "0,1")
            {
                radioButton0.Checked = true;
            }
            else if (whichToSelect == "1,0")
            {
                radioButton1.Checked = true;
            }
            radioButton0.Click += SaveOrder01;
            radioButton1.Click += SaveOrder10;
            switchStats.Click += Change;
        }

        private void SaveOrder01(object sender, EventArgs eventArgs)
        {
            WriteOrder("0,1");
        }
        private void SaveOrder10(object sender, EventArgs eventArgs)
        {
            WriteOrder("1,0");
        }
        private void Change(object sender, EventArgs eventArgs)
        {
            if (switchStats.Checked == true)
            {
                WriteStatsShow("1");
            }
            else
            {
                WriteStatsShow("0");
            }
        }
        public string ReadOrderFromUserSettings()
        {
            string file = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "UserSettings.csv");
            StreamReader reader = new StreamReader(file);
            string txt;
            string order = "";
            while ((txt = reader.ReadLine()) != null)
            {
                string[] words = txt.Split(';');
                if (words[0] == "order")
                {
                    order = words[1];
                }
            }
            reader.Close();
            return order;
        }
        public void WriteOrder(string order)
        {
            string[] arrLine = File.ReadAllLines(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "UserSettings.csv"));
            arrLine[0] = "order;" + order;
            File.WriteAllLines(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "UserSettings.csv"), arrLine);
        }
        public bool ReadIfStatsShouldBeShownFromUserSettings()
        {
            string file = "UserSettings.csv";
            var currentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            file = Path.Combine(currentPath, file);
            StreamReader reader = new StreamReader(file);
            string txt;
            bool returnValue = true;
            while ((txt = reader.ReadLine()) != null)
            {
                string[] words = txt.Split(';');
                if (words[0] == "showStatsAfterSession")
                {
                    if (int.Parse(words[1]) == 1)
                    {
                        returnValue = true;
                    }
                    else
                    {
                        returnValue = false;
                    }
                }
            }
            reader.Close();
            return returnValue;
        }
        public void WriteStatsShow(string order)
        {
            string[] arrLine = File.ReadAllLines(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "UserSettings.csv"));
            arrLine[1] = "showStatsAfterSession;" + order;
            File.WriteAllLines(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "UserSettings.csv"), arrLine);
        }
    }
}