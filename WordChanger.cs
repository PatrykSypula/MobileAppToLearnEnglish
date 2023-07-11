using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Service.Autofill;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Java.Security;
using Java.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using static Android.Telephony.CarrierConfigManager;
using static Android.Views.ViewGroup;
using static System.Net.Mime.MediaTypeNames;
using static Xamarin.Essentials.Platform;
using Path = System.IO.Path;

namespace Nauka_angielskiego
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class WordChanger : AppCompatActivity
    {
        string fileName;
        string word;
        string wordLine;
        EditText editTextPolish;
        EditText editTextEnglish;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WordChanger);
            fileName = Intent.GetStringExtra("fileName");
            word = Intent.GetStringExtra("word");
            wordLine = Intent.GetStringExtra("wordLine");

            var textView = FindViewById<TextView>(Resource.Id.WordChangerTextView);
            string[] split = word.Split(";");
            textView.Text = "Zamień słowo " + split[0] + " - " + split[1];

            editTextPolish = FindViewById<EditText>(Resource.Id.WordChangerPolish);
            editTextPolish.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
            };

            editTextEnglish = FindViewById<EditText>(Resource.Id.WordChangerEnglish);
            editTextEnglish.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
            };
            var buttonOverride = FindViewById<Button>(Resource.Id.WordChangerButtonChange);
            buttonOverride.Click += Override;
            buttonOverride.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);
        }
        private void Override(object sender, EventArgs eventArgs)
        {
            if (editTextPolish.Text.Length > 0 && editTextEnglish.Text.Length > 0)
            {
                string[] arrLine = File.ReadAllLines(Path.Combine(Globals.DictionaryPath,fileName));
                arrLine[int.Parse(wordLine)] = editTextPolish.Text.Trim() + ";" + editTextEnglish.Text.Trim();
                File.WriteAllLines(Path.Combine(Globals.DictionaryPath, fileName), arrLine);
                Globals.ShortToast("Słowo zostało zmienione.");
                Finish();
            }
            else
            {
                Globals.ShortToast("Proszę wpisać dane do zamiany.");
            }
        }
    }
}