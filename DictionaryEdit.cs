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
using AndroidX.Annotations;
using AndroidX.AppCompat.App;
using Java.Security;
using Java.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using static Android.Telephony.CarrierConfigManager;
using static Android.Views.ViewGroup;
using Path = System.IO.Path;

namespace Nauka_angielskiego
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class DictionaryEdit : AppCompatActivity
    {
        string fileName;
        EditText polishWord;
        EditText englishWord;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DictionaryEdit);

            Button buttonWordAdd = FindViewById<Button>(Resource.Id.buttonWordAdd);
            buttonWordAdd.Click += AddWord;
            buttonWordAdd.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            Button buttonWordEdit = FindViewById<Button>(Resource.Id.buttonWordEdit);
            buttonWordEdit.Click += EditWord;
            buttonWordEdit.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            Button buttonWordRemove = FindViewById<Button>(Resource.Id.buttonWordRemove);
            buttonWordRemove.Click += RemoveWord;
            buttonWordRemove.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            //Button buttonWordSaveDictionary = FindViewById<Button>(Resource.Id.buttonWordSaveDictionary);
            //buttonWordSaveDictionary.Click += Exit;
            //buttonWordSaveDictionary.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            fileName = Intent.GetStringExtra("fileName");
            polishWord = FindViewById<EditText>(Resource.Id.TextViewPolishWord);
            englishWord = FindViewById<EditText>(Resource.Id.TextViewEnglishWord);
        }

        private void AddWord(object sender, EventArgs eventArgs)
        {
            string file;
            file = Path.Combine(Globals.DictionaryPath, fileName);
            if (polishWord.Text.Trim().Length != 0 && englishWord.Text.Trim().Length != 0)
            {
                try
                {
                    StreamWriter writer;
                    writer = new StreamWriter(file, true);
                    writer.WriteLine(polishWord.Text.Trim()+";"+englishWord.Text.Trim());
                    writer.Close();
                    Globals.ShortToast("Dodano słowo "+polishWord.Text.Trim().Substring(0,1).ToUpper()+ polishWord.Text.Trim().Substring(1, polishWord.Text.Trim().Length-1).ToLower() + " - "+ englishWord.Text.Trim().Substring(0, 1).ToUpper()+ englishWord.Text.Trim().Substring(1, englishWord.Text.Trim().Length-1).ToLower());
                    polishWord.Text = "";
                    englishWord.Text = "";
                }
                catch
                {
                    Globals.ShortToast("Wystąpił błąd podczas dodawania.");
                }
            }
            else
            {
                Globals.ShortToast("Wprowadzone słowo nie może być puste.");
            }
        }
        private void EditWord(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(this, typeof(SelectWordButtons));
            intent.PutExtra("fileName", fileName);
            intent.PutExtra("action", "edit");
            StartActivity(intent);
        }
        private void RemoveWord(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(this, typeof(SelectWordButtons));
            intent.PutExtra("fileName", fileName);
            intent.PutExtra("action", "remove");
            StartActivity(intent);
        }
        //private void Exit(object sender, EventArgs eventArgs)
        //{
        //    Finish();
        //}
    }
}