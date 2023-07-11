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
    public class DictionariesDraw : AppCompatActivity
    {
        string selectedOption;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DrawButtonsWithAllDictionaries);
            selectedOption = Intent.GetStringExtra("selectedOption");
            DrawButtonsWithFileNames();
        }

        public void DrawButtonsWithFileNames()
        {
            DirectoryInfo d = new DirectoryInfo(Globals.DictionaryPath);
            FileInfo[] Files = d.GetFiles("*.csv");
            string[] filesFromAsset = Globals.GetFiles();
            string[] filesFromAssetPolish = Globals.GetFilesPolish();
            int i = 0;
            foreach (string file in filesFromAsset)
            {
                Button myButton = new Button(this, null, 0, Resource.Style.buttonTheme);
                myButton.Text = filesFromAssetPolish[i].Substring(0, filesFromAssetPolish[i].Length -4);
                LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.LinearLayoutDictionariesDraw);
                LayoutParams lp = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                ll.AddView(myButton, lp);
                myButton.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);
                myButton.Click += delegate
                {
                    switch (selectedOption)
                    {
                        case "Copy":
                            Intent intentCopy = new Intent(this, typeof(DictionaryAdd));
                            intentCopy.PutExtra("copiedFilePath", Path.Combine(Globals.DictionaryPath,file));
                            intentCopy.PutExtra("isAsset", "1");
                            intentCopy.PutExtra("fileName", file);
                            StartActivity(intentCopy);
                            Finish();
                            break;

                        case "Edit":
                            Globals.ShortToast("Nie można edytować standardowego słownika. Spróbuj skopiować ten słownik aby dokonać zmian.");
                            break;

                        case "Remove":
                            Globals.ShortToast("Nie można usunąć standardowego słownika");
                            break;

                    }
                };
                i++;
            }
            foreach (FileInfo filee in Files)
            {
                if (!(filee.Name == "UserSettings.csv"))
                {
                    Button myButton = new Button(this, null, 0, Resource.Style.buttonTheme);
                    myButton.Text = filee.Name.Substring(0, filee.Name.Length - 4);
                    LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.LinearLayoutDictionariesDraw);
                    LayoutParams lp = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                    ll.AddView(myButton, lp);
                    myButton.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);
                    myButton.Click += delegate
                    {
                        switch (selectedOption)
                        {
                            case "Copy":
                                Intent intentCopy = new Intent(this, typeof(DictionaryAdd));
                                intentCopy.PutExtra("copiedFilePath", Path.Combine(Globals.DictionaryPath, filee.Name));
                                intentCopy.PutExtra("isAsset", "0");
                                intentCopy.PutExtra("fileName", filee.Name);
                                StartActivity(intentCopy);
                                Finish();
                                break;

                            case "Edit":
                                Intent intent = new Intent(this, typeof(DictionaryEdit));
                                intent.PutExtra("fileName", filee.Name);
                                StartActivity(intent);
                                Finish();
                                break;

                            case "Remove":
                                try
                                {
                                    string filePath = Path.Combine(Globals.DictionaryPath, filee.Name);

                                    Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this, Resource.Style.buttonWhiteBackgroud);
                                    Android.App.AlertDialog alert = dialog.Create();
                                    alert.SetTitle("Potwierdzenie usunięcia");
                                    alert.SetMessage("Czy na pewno chcesz usunąć słownik " + filee.Name.Substring(0, filee.Name.Length - 4) + "?");
                                    alert.SetButton("Tak", (c, ev) =>
                                    {
                                        File.Delete(filePath);
                                        ll.RemoveView(myButton);
                                    });
                                    alert.SetButton2("Nie", (c, ev) => { });
                                    alert.Show();
                                    Button positive = alert.GetButton((int)DialogButtonType.Positive);
                                    positive.SetTextColor(Color.Black);
                                    Button negative = alert.GetButton((int)DialogButtonType.Negative);
                                    negative.SetTextColor(Color.Black);

                                }
                                catch (IOException e)
                                {
                                    Globals.ShortToast($"File could not be deleted:");
                                    Globals.ShortToast(e.Message);
                                }
                                break;

                        }
                    };
                }
            }
        }
    }
}