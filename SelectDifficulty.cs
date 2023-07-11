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
    public class SelectDifficulty : AppCompatActivity
    {
        public LinearLayout layoutNavigation;
        public LinearLayout layoutButtonPanel;
        public ScrollView scrollViewButtons;
        public bool IsOnMainMenuScreen = true;
        public bool DidGenerateButtons = false;
        string selectedDifficulty;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SelectDifficulty);

            Button buttonUczSie = FindViewById<Button>(Resource.Id.buttonUczSie);
            buttonUczSie.Click += UczSie;
            buttonUczSie.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            Button buttonSprawdzSie = FindViewById<Button>(Resource.Id.buttonSprawdzSie);
            buttonSprawdzSie.Click += SprawdzSie;
            buttonSprawdzSie.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            Button buttonEgzamin = FindViewById<Button>(Resource.Id.buttonEgzamin);
            buttonEgzamin.Click += Egzamin;
            buttonEgzamin.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            layoutNavigation = FindViewById<LinearLayout>(Resource.Id.LinearLayoutNavigate);
            layoutButtonPanel = FindViewById<LinearLayout>(Resource.Id.LinearLayoutButtonPanel);
            scrollViewButtons = FindViewById<ScrollView>(Resource.Id.ScrollViewButtons);

            ShowMenu();
        }

        private void UczSie(object sender, EventArgs eventArgs)
        {
            AfterSelectedDifficulty();
            selectedDifficulty = "1";
        }

        private void SprawdzSie(object sender, EventArgs eventArgs)
        {
            AfterSelectedDifficulty();
            selectedDifficulty = "2";
        }

        private void Egzamin(object sender, EventArgs eventArgs)
        {
            AfterSelectedDifficulty();
            selectedDifficulty = "3";
        }
        public void AfterSelectedDifficulty()
        {
            HideMenu();
            if (!DidGenerateButtons)
            {
                DrawButtonsWithFileNames();
            }
        }

        public override void OnBackPressed()
        {
            if(IsOnMainMenuScreen)
            {
                base.OnBackPressed();
            }
            else
            {
                ShowMenu();
            }
        }
        public void DrawButtonsWithFileNames()
        {
            DirectoryInfo d = new DirectoryInfo(Globals.DictionaryPath);
            FileInfo[] Files = d.GetFiles("*.csv");

            string[] filesFromAsset = Globals.GetFiles();
            string[] filesFromAssetPolish = Globals.GetFilesPolish();

            int i = 0;
            //Słowniki standardowe (z assetów)
            foreach (string file in filesFromAsset)
            {
                Button myButton = new Button(this, null, 0, Resource.Style.buttonTheme);
                myButton.Text = filesFromAssetPolish[i].Substring(0, filesFromAssetPolish[i].Length - 4);
                LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.LinearLayoutButtonPanel);
                LayoutParams lp = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                ll.AddView(myButton, lp);
                myButton.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);
                myButton.Click += delegate
                {
                    StartSession(file, "1", selectedDifficulty);
                };
                i++;
            }
            //Słowniki stworzone przez użytkownika
            foreach (FileInfo filee in Files)
            {
                if(!(filee.Name == "UserSettings.csv"))
                {
                    Button myButton = new Button(this, null, 0, Resource.Style.buttonTheme);
                    myButton.Text = filee.Name.Substring(0, filee.Name.Length - 4);
                    LinearLayout ll = (LinearLayout)FindViewById(Resource.Id.LinearLayoutButtonPanel);
                    LayoutParams lp = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                    ll.AddView(myButton, lp);
                    myButton.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);
                    myButton.Click += delegate
                    {
                        StartSession(filee.Name, "0", selectedDifficulty);
                    };
                }
                DidGenerateButtons = true;
            }
        }
        public void StartSession(string fileName, string isFromAsset, string selectedDifficulty)
        {
            if (selectedDifficulty == "1")
            {
                Finish();
                Intent intent = new Intent(this, typeof(Session));
                intent.PutExtra("FileName", fileName);
                intent.PutExtra("IsFromAsset", isFromAsset);
                intent.PutExtra("SelectedDifficulty", selectedDifficulty);
                StartActivity(intent);
            }
            else
            {
                Finish();
                Intent intent = new Intent(this, typeof(SessionInput));
                intent.PutExtra("FileName", fileName);
                intent.PutExtra("IsFromAsset", isFromAsset);
                intent.PutExtra("SelectedDifficulty", selectedDifficulty);
                StartActivity(intent);
            }
                
        }
        public void ShowMenu()
        {
            try
            {
                layoutNavigation.Visibility = ViewStates.Visible;
                scrollViewButtons.Visibility = ViewStates.Invisible;
                layoutButtonPanel.Visibility = ViewStates.Invisible;
                IsOnMainMenuScreen = true;
            }
            catch
            {
                Toast.MakeText(Application.Context, "Wystąpił błąd podczas ładowania okienka", ToastLength.Short).Show();
            }
        }
        public void HideMenu()
        {
            try
            {
                layoutNavigation.Visibility = ViewStates.Invisible;
                scrollViewButtons.Visibility = ViewStates.Visible;
                layoutButtonPanel.Visibility = ViewStates.Visible;
                IsOnMainMenuScreen = false;
            }
            catch
            {
                Toast.MakeText(Application.Context, "Wystąpił błąd podczas ładowania okienka", ToastLength.Short).Show();
            }
        }
    }
}