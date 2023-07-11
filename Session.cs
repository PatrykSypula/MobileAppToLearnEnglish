using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.IO;
using Path = System.IO.Path;

namespace Nauka_angielskiego
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class Session : AppCompatActivity
    {
        CardStack words;
        Word currentWord;
        TextView buttonTop;
        TextView buttonBottom;
        int totalShown = 0;
        int goodAnswer = 0;
        int selectedDifficulty;
        string fileName;
        string[] wordHolder;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Session);

            buttonTop = FindViewById<TextView>(Resource.Id.buttonWordTop);
            buttonTop.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            buttonBottom = FindViewById<TextView>(Resource.Id.buttonWordBottom);
            buttonBottom.Click += ShowWord;
            buttonBottom.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            Button buttonUmiem = FindViewById<Button>(Resource.Id.buttonUmiem);
            buttonUmiem.Click += IKnow;
            buttonUmiem.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            Button buttonNieUmiem = FindViewById<Button>(Resource.Id.buttonNieUmiem);
            buttonNieUmiem.Click += IDoNotKnow;
            buttonNieUmiem.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            AssetManager assets = this.Assets;
            fileName = Intent.GetStringExtra("FileName");
            string temp = Intent.GetStringExtra("IsFromAsset");
            int isFromAsset = int.Parse(temp);
            temp = Intent.GetStringExtra("SelectedDifficulty");
            selectedDifficulty = int.Parse(temp);
            int[] order = ReadOrderFromUserSettings();
            if (isFromAsset == 1)
            {
                using (StreamReader sr = new StreamReader(assets.Open(fileName)))
                {
                    var list = new CardStack();
                    string txt;
                    while ((txt = sr.ReadLine()) != null)
                    {
                        string[] words = txt.Split(';');
                        list.AddWord(new Word(words[order[0]], words[order[1]]));
                    }
                    sr.Close();
                    words = list;
                }
            }
            else
            {
                ReadCardStack(order, fileName);
            }
            try
            {
                AfterClick();
            }
            catch
            {
                Globals.ShortToast("Ten słownik nie ma żadnych słów");
                Finish();
            }

        }
        private void IKnow(object sender, EventArgs eventArgs)
        {
            words.RemoveWord(words.index);
            goodAnswer++;
            CheckIfSessionHasEnded();
        }
        private void IDoNotKnow(object sender, EventArgs eventArgs)
        {
            currentWord = words.RollWord();
            CheckIfSessionHasEnded();

        }
        private void ShowWord(object sender, EventArgs eventArgs)
        {
            buttonBottom.Text = currentWord.englishWord;
        }
        public void AfterClick()
        {
            totalShown++;
            currentWord = words.PrintWord();
            wordHolder = currentWord.ToString().Split(',');
            buttonTop.Text = currentWord.polishWord;
            buttonBottom.Text = "";
        }

        public void ReadCardStack(int[] order, string file)
        {
            file = Path.Combine(Globals.DictionaryPath, file);
            StreamReader reader = new StreamReader(file);
            var list = new CardStack();
            string txt;
            while ((txt = reader.ReadLine()) != null)
            {
                string[] words = txt.Split(';');
                list.AddWord(new Word(words[order[0]], words[order[1]]));
            }
            reader.Close();
            words = list;
        }
        public int[] ReadOrderFromUserSettings()
        {
            string file = "UserSettings.csv";
            var currentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            var nameholder = file;
            int[] order = new int[2];
            file = Path.Combine(currentPath, nameholder);
            StreamReader reader = new StreamReader(file);
            string txt;
            while ((txt = reader.ReadLine()) != null)
            {
                string[] words = txt.Split(';');
                if (words[0] == "order")
                {
                    string[] stringOrder = words[1].Split(",");
                    order[0] = int.Parse(stringOrder[0]);
                    order[1] = int.Parse(stringOrder[1]);
                }
            }
            reader.Close();
            return order;

        }
        public int ReadIfStatsShouldBeShownFromUserSettings()
        {
            string file = "UserSettings.csv";
            var currentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            file = Path.Combine(currentPath, file);
            StreamReader reader = new StreamReader(file);
            string txt;
            int returnValue = 2;
            while ((txt = reader.ReadLine()) != null)
            {
                string[] words = txt.Split(';');
                if (words[0] == "showStatsAfterSession")
                {
                    returnValue = int.Parse(words[1]);
                }
            }
            reader.Close();
            return returnValue;

        }
        public void CheckIfSessionHasEnded()
        {
            if (words.GetList().Count == 0)
            {
                if (ReadIfStatsShouldBeShownFromUserSettings() == 1)
                {
                    Intent intent = new Intent(this, typeof(Statistics));
                    intent.PutExtra("answersGood", goodAnswer.ToString());
                    intent.PutExtra("answersShown", totalShown.ToString());
                    StartActivity(intent);
                }
                WriteStatistics(fileName);
                Finish();
            }
            else AfterClick();
        }
        public override void OnBackPressed()
        {
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this, Resource.Style.buttonWhiteBackgroud);
            Android.App.AlertDialog alert = dialog.Create();
            alert.SetTitle("Potwierdzenie wyjścia");
            alert.SetMessage("Czy na pewno chcesz przerwać sesję?");
            alert.SetButton("Tak", (c, ev) =>
            {
                base.OnBackPressed();
            });
            alert.SetButton2("Nie", (c, ev) => { });
            alert.Show();
            Button positive = alert.GetButton((int)DialogButtonType.Positive);
            positive.SetTextColor(Color.Black);
            Button negative = alert.GetButton((int)DialogButtonType.Negative);
            negative.SetTextColor(Color.Black);

        }
        public void WriteStatistics(string file)
        {
            try
            {
                var currentPath = Globals.StatisticsPath;
                var nameholder = ChangeAssetNameToPolish(file);
                file = Path.Combine(currentPath, nameholder);

                StreamWriter writer;
                if (!File.Exists(file))
                {
                    writer = File.CreateText(file);
                }
                else
                {
                    writer = new StreamWriter(file, true);
                }
                writer.WriteLine(selectedDifficulty.ToString() + ";" + totalShown.ToString() + ";" 
                    + goodAnswer.ToString() + ";" + DateTime.Now.ToString("dd/MM/yyyy"));
                writer.Close();
            }
            catch (Exception ex)
            {
                Globals.ShortToast("Wystąpił błąd podczas zapisywania statystyk.");
            }
        }
        public string ChangeAssetNameToPolish(string name)
        {
            string returnString = "";
            string[] filesFromAsset = Globals.GetFiles();
            string[] filesFromAssetPolish = Globals.GetFilesPolish();
            int i = 0;
            foreach (string file in filesFromAsset)
            {
                if (name == file)
                {
                    returnString = filesFromAssetPolish[i];
                    break;
                }
                else
                {
                    returnString = name;
                }
                i++;
            }
            return returnString;

        }
    }
}