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
    public class DictionariesMenu : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DictionariesMenu);

            Button buttonDodaj = FindViewById<Button>(Resource.Id.buttonDictionaryAdd);
            buttonDodaj.Click += Add;
            buttonDodaj.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            Button buttonKopiuj = FindViewById<Button>(Resource.Id.buttonDictionaryCopy);
            buttonKopiuj.Click += Copy;
            buttonKopiuj.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);

            Button buttonEdytuj = FindViewById<Button>(Resource.Id.buttonDictionaryEdit);
            buttonEdytuj.Click += Edit;
            buttonEdytuj.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);
            
            Button buttonUsun = FindViewById<Button>(Resource.Id.buttonDictionaryRemove);
            buttonUsun.Click += Remove;
            buttonUsun.SetShadowLayer(3, 2, 2, Android.Graphics.Color.Black);
        }

        private void Add(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(this, typeof(DictionaryAdd));
            StartActivity(intent);
            Finish();
        }
        private void Copy(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(this, typeof(DictionariesDraw));
            intent.PutExtra("selectedOption", "Copy");
            StartActivity(intent);
            Finish();
        }
        private void Edit(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(this, typeof(DictionariesDraw));
            intent.PutExtra("selectedOption", "Edit");
            StartActivity(intent);
            Finish();
        }
        private void Remove(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(this, typeof(DictionariesDraw));
            intent.PutExtra("selectedOption", "Remove");
            StartActivity(intent);
            Finish();
        }
    }
}