using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace Nauka_angielskiego
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = false)]
    public class Statistics : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Statistics);
            var answersGood = FindViewById<TextView>(Resource.Id.GoodAnswers);
            var answersShown = FindViewById<TextView>(Resource.Id.AllAnswers);
            var answersPercentage = FindViewById<TextView>(Resource.Id.PercentageAnswer);

            string goodAnswer = Intent.GetStringExtra("answersGood");
            int temp1 = int.Parse(goodAnswer);
            answersGood.Text = temp1.ToString();

            string shownAnswer = Intent.GetStringExtra("answersShown");
            int temp2 = int.Parse(shownAnswer);
            answersShown.Text = temp2.ToString();
            
            if(Intent.HasExtra("selectedDifficulty"))
            {
                string selectedDifficulty = Intent.GetStringExtra("selectedDifficulty");
                string date = Intent.GetStringExtra("date");
                string fileName = Intent.GetStringExtra("fileName");
                var mainText = FindViewById<TextView>(Resource.Id.StatsMainText);
                mainText.Text = selectedDifficulty;
                var secondaryText = FindViewById<TextView>(Resource.Id.StatsSecondaryText);
                secondaryText.Text = "Statystyki słownika "+fileName+" z dnia "+date;

            }

            answersPercentage.Text = Math.Round(((double)temp1/(double)temp2)*100, 2).ToString() + "%";
        }
    }
}