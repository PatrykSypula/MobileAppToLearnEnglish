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
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using static Android.Telephony.CarrierConfigManager;
using static Android.Views.ViewGroup;
using Path = System.IO.Path;

namespace Nauka_angielskiego
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class DictionaryAdd : AppCompatActivity
    {
        List<string> list;
        bool isCopied = false;
        string copyPath;
        string isAsset;
        string fileName;
        EditText inputText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DictionaryAdd);
            list = Globals.GetListOfFileNames();

            if (Intent.HasExtra("copiedFilePath"))
            {
                copyPath = Intent.GetStringExtra("copiedFilePath");
                isAsset = Intent.GetStringExtra("isAsset");
                fileName = Intent.GetStringExtra("fileName");
                isCopied = true;
            }

            inputText = FindViewById<EditText>(Resource.Id.TextViewDictionaryNameCreate);
            inputText.KeyPress += (object sender, View.KeyEventArgs e) => {
                e.Handled = false;
            };

            var buttonAdd = FindViewById<Button>(Resource.Id.buttonCreateDictionary);
            buttonAdd.Click += AddDictionary;
            buttonAdd.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);
        }
        private void AddDictionary(object sender, EventArgs eventArgs)
        {
            bool didDuplicate = false;
            foreach (string txt in list)
            {
                if (inputText.Text.ToLower().Trim() == txt.Substring(0, txt.Length - 4).ToLower().Trim())
                {
                    Globals.ShortToast("Słownik o takiej nazwie już istnieje");
                    didDuplicate = true;
                    break;

                }
            }
            if (!didDuplicate)
            {
                if (isCopied)
                {
                    if(isAsset == "1")
                    {
                        AssetManager assets = this.Assets;
                        StreamWriter writer;
                        string txt;
                        StreamReader sr = new StreamReader(assets.Open(fileName));
                        writer = File.CreateText(Path.Combine(Globals.DictionaryPath, inputText.Text+".csv"));
                        while ((txt = sr.ReadLine()) != null)
                        {
                            writer.WriteLine(txt);
                        }
                        writer.Close();
                        sr.Close();
                        Intent intentCopy = new Intent(this, typeof(DictionaryEdit));
                        intentCopy.PutExtra("fileName", Path.Combine(Globals.DictionaryPath, inputText.Text + ".csv"));
                        StartActivity(intentCopy);
                        Finish();
                        Globals.ShortToast("Utworzono słownik " + inputText.Text.Trim());
                    }   
                    else
                    {     
                    File.Copy(copyPath, Path.Combine(Globals.DictionaryPath, inputText.Text + ".csv"));
                    Intent intentCopy = new Intent(this, typeof(DictionaryEdit));
                    intentCopy.PutExtra("fileName", Path.Combine(Globals.DictionaryPath, inputText.Text + ".csv"));
                    StartActivity(intentCopy);
                    Finish();
                    Globals.ShortToast("Utworzono słownik " + inputText.Text.Trim());
                    }
                }
                else
                {
                    if (!File.Exists(Path.Combine(Globals.DictionaryPath, inputText.Text)))
                    {
                        StreamWriter writer;
                        writer = File.CreateText((Path.Combine(Globals.DictionaryPath, inputText.Text + ".csv")).ToLower().Trim());
                        writer.Close();
                        Intent intent = new Intent(this, typeof(DictionaryEdit));
                        intent.PutExtra("fileName", Path.Combine(Globals.DictionaryPath, inputText.Text + ".csv"));
                        StartActivity(intent);
                        Finish();
                        Globals.ShortToast("Utworzono słownik " + inputText.Text);
                    }
                }
            }
        }

    }
}