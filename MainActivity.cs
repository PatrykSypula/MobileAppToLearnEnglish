using System;
using Android.App;
using Android.OS;
using AndroidX.AppCompat.App;
using System.IO;
using Android.Widget;
using Android.Content.Res;
using Android.Content;

namespace Nauka_angielskiego
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            //SetSupportActionBar(toolbar);
            System.IO.Directory.CreateDirectory(Globals.DictionaryPath);
            System.IO.Directory.CreateDirectory(Globals.StatisticsPath);

            CreateUserSettingsIfDoesNotExists();

            Button buttonNauka = FindViewById<Button>(Resource.Id.buttonNauka);
            buttonNauka.Click += Nauka;
            buttonNauka.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            Button buttonStatystyki = FindViewById<Button>(Resource.Id.buttonStatystyki);
            buttonStatystyki.Click += Statystyki;
            buttonStatystyki.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            Button buttonSlowniki = FindViewById<Button>(Resource.Id.buttonSlowniki);
            buttonSlowniki.Click += Slowniki;
            buttonSlowniki.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            Button buttonUstawienia = FindViewById<Button>(Resource.Id.buttonUstawienia);
            buttonUstawienia.Click += Ustawienia;
            buttonUstawienia.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

        }
        private void Nauka(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(this, typeof(SelectDifficulty));
            StartActivity(intent);
        }

        private void Statystyki(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(this, typeof(StatsChooseDictionary));
            StartActivity(intent);
        }
        private void Slowniki(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(this, typeof(DictionariesMenu));
            StartActivity(intent);
        }

        private void Ustawienia(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(this, typeof(Settings));
            StartActivity(intent);
        }
        public void CreateUserSettingsIfDoesNotExists()
        {
            string file = "UserSettings.csv";
            var currentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            var nameholder = file;
            file = Path.Combine(currentPath, nameholder);
            if (!File.Exists(file))
            {
                AssetManager assets = this.Assets;
                StreamWriter writer;
                string txt;
                StreamReader sr = new StreamReader(assets.Open("DefaultSettings.csv"));
                writer = File.CreateText(file);
                while ((txt = sr.ReadLine()) != null)
                {
                    writer.WriteLine(txt);
                }
                writer.Close();
                sr.Close();
            }
        }
    }
}
