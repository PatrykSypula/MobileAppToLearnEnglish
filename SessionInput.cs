using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.IO;
using Path = System.IO.Path;

namespace Nauka_angielskiego
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class SessionInput : AppCompatActivity
    {
        CardStack words;
        Word currentWord;
        TextView buttonTop;
        EditText buttonBottom;
        int selectedDifficulty;
        int totalShown = 0;
        int goodAnswer = 0;
        string fileName;
        string[] wordHolder;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SessionInput);

            buttonTop = FindViewById<TextView>(Resource.Id.textViewTop);
            buttonTop.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            buttonBottom = FindViewById<EditText>(Resource.Id.TextViewBottom);
            //buttonBottom.Click += ShowWord;
            buttonBottom.KeyPress += (object sender, View.KeyEventArgs e) =>
            {
                e.Handled = false;
                if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
                {
                    CheckAnswer();
                    e.Handled = true;
                }
            };
            buttonBottom.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            Button buttonUmiem = FindViewById<Button>(Resource.Id.buttonCheck);
            buttonUmiem.Click += Check;
            buttonUmiem.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            AssetManager assets = this.Assets;
            fileName = Intent.GetStringExtra("FileName");
            string temp = Intent.GetStringExtra("IsFromAsset");
            int isFromAsset = int.Parse(temp);
            temp = Intent.GetStringExtra("SelectedDifficulty");
            selectedDifficulty = int.Parse(temp);
            if (selectedDifficulty == 2)
            {
                buttonUmiem.SetText(Resource.String.Check);
            }
            else
            {
                buttonUmiem.SetText(Resource.String.Next);
            }
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
        private void Check(object sender, EventArgs eventArgs)
        {
            CheckAnswer();
        }
        public void CheckAnswer()
        {
            //Ze zwracaniem
            if (selectedDifficulty == 2)
            {
                if (currentWord.englishWord.ToLower().Trim() == buttonBottom.Text.ToLower().Trim())
                {
                    words.RemoveWord(words.index);
                    goodAnswer++;
                }
                else
                {
                    Toast.MakeText(Android.App.Application.Context, "Odpowiedź błędna. Poprawna odpowiedź to " + currentWord.englishWord.Substring(0, 1).ToUpper() + currentWord.englishWord.Substring(1, currentWord.englishWord.Length - 1).ToLower(), ToastLength.Short).Show();
                    currentWord = words.RollWord();
                }
            }
            //Bez zwracania
            else
            {
                if (currentWord.englishWord.ToLower().Trim() == buttonBottom.Text.ToLower().Trim())
                {
                    words.RemoveWord(words.index);
                    goodAnswer++;
                }
                else
                {
                    words.RemoveWord(words.index);
                }
            }
            CheckIfSessionHasEnded();
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
            StreamReader reader = new StreamReader(Path.Combine(Globals.DictionaryPath, file));
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
            var list = new CardStack();
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
            var nameholder = file;
            file = Path.Combine(currentPath, nameholder);
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