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
using static Android.Provider.UserDictionary;
using static Android.Telephony.CarrierConfigManager;
using static Android.Views.ViewGroup;
using Path = System.IO.Path;

namespace Nauka_angielskiego
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class SelectWordButtons : AppCompatActivity
    {
        string fileName;
        string action;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectWordButtons);

            fileName = Intent.GetStringExtra("fileName");
            action = Intent.GetStringExtra("action");

            DrawButtonsWithWords();
        }

        public void DrawButtonsWithWords()
        {
            StreamReader reader = new StreamReader(Path.Combine(Globals.DictionaryPath, fileName));
            //var list = new CardStack();
            string txt;
            int line = 0;
            while ((txt = reader.ReadLine()) != null)
            {
                LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.LinearLayoutSelectWord);
                LayoutParams lp = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                Button myButton = new Button(this, null, 0, Resource.Style.buttonTheme);
                string[] words = txt.Split(';');
                myButton.Text = words[0] + " - " + words[1];
                ll.AddView(myButton, lp);
                myButton.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);
                int wordLine = line;
                string txtHolder = txt;
                myButton.Click += delegate
                {
                    if (action == "edit")
                    {
                        Intent intent = new Intent(this, typeof(WordChanger));
                        intent.PutExtra("fileName", fileName);
                        intent.PutExtra("word", txtHolder.ToString());
                        intent.PutExtra("wordLine", wordLine.ToString());
                        StartActivity(intent);
                        Finish();
                    }
                    else
                    {
                        Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this, Resource.Style.buttonWhiteBackgroud);
                        Android.App.AlertDialog alert = dialog.Create();
                        alert.SetTitle("Potwierdzenie usunięcia");
                        alert.SetMessage("Czy na pewno chcesz usunąć słowo "+myButton.Text+"?");
                        alert.SetButton("Tak", (c, ev) =>
                        {
                            string[] arrLine = File.ReadAllLines(Path.Combine(Globals.DictionaryPath, fileName));
                            arrLine[wordLine] = "";
                            int newArrayIntLine = 0;
                            string[] newArrLine = new string[arrLine.Length - 1];
                            for (int i = 0; i < arrLine.Length; i++)
                            {
                                if (arrLine[i] != "")
                                {
                                    newArrLine[newArrayIntLine] = arrLine[i];
                                    newArrayIntLine++;
                                }
                            }
                            File.WriteAllLines(Path.Combine(Globals.DictionaryPath, fileName), newArrLine);
                            Globals.ShortToast("Słowo zostało usuniete");
                            Finish();
                        });
                        alert.SetButton2("Nie", (c, ev) => { });
                        alert.Show();
                        Button positive = alert.GetButton((int)DialogButtonType.Positive);
                        positive.SetTextColor(Color.Black);
                        Button negative = alert.GetButton((int)DialogButtonType.Negative);
                        negative.SetTextColor(Color.Black);
                    }
                };
                line++;
                //list.AddWord(new Word(words[order[0]], words[order[1]]));
            }
            reader.Close();
            //words = list;
        }
    }
}