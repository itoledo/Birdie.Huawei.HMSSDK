using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Huawei.Hms.Common;
using Com.Huawei.Hms.Support.Hwid;
using Com.Huawei.Hms.Support.Hwid.Request;
using Com.Huawei.Hms.Support.Hwid.Service;

namespace Birdie.Huawei.HMSSDK.Demo
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, Name= "cl.birdie.huawei.hmssdk.demo.MainActivity")]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            //View view = (View) sender;
            //Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
            //    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            HuaweiIdAuthParams authParams = new HuaweiIdAuthParamsHelper(HuaweiIdAuthParams.DefaultAuthRequestParam).SetIdToken().CreateParams();
            IHuaweiIdAuthService service = HuaweiIdAuthManager.GetService(this, authParams);
            StartActivityForResult(service.SignInIntent, 8888);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        const string TAG = "MainActivity";

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 8888)
            {
                var bundle = data.Extras;
                Dictionary<string, object> dict = bundle.KeySet()
                    .ToDictionary<string, string, object>(key => key, key => bundle.Get(key));
                foreach (var d in dict)
                    System.Diagnostics.Debug.WriteLine($"{d.Key} -> {d.Value}");
                //var authHuaweiIdTask = HuaweiIdAuthManager.ParseAuthResultFromIntent(data);
                //if (authHuaweiIdTask.Successful)
                //{
                //    //The sign-in is successful, and the user's HUAWEI ID information and ID token are obtained.
                //    var huaweiAccount = authHuaweiIdTask.Result;
                //    Log.Info(TAG, "idToken:" + huaweiAccount.IdToken);
                //}
                //else
                //{
                //    //The sign-in failed.      
                //    Log.Error(TAG, "sign in failed : " + ((ApiException)authHuaweiIdTask.Exception).StatusCode);
                //}

// [0:] HMS_FOREGROUND_RES_UI -> {"uiDuration":32070}
// [0:] HUAWEIID_SIGNIN_RESULT -> {"signInHuaweiId":{"photoUriString":"","expirationTimeSecs":0,"grantedScopes":[{"mScopeUri":"openid"},{"mScopeUri":"profile"}],"displayName":"BirdieCL","familyName":"","gender":-1,"givenName":"","idToken":"eyJraWQiOiI1MjJhZTRlNzhlODNkMjBkYmNiNzEyZjViZDFkZTMyYzcxN2E5YWJiYzczMGViMDAzNjgyOTVkYTA5YzA4NjczIiwidHlwIjoiSldUIiwiYWxnIjoiUlMyNTYifQ.eyJhdF9oYXNoIjoiMXkwaWVYTGxYSVFNZG12NUQ3ZGlaUSIsImF1ZCI6IjEwMTkzNzM3MSIsInN1YiI6Ik1ER3RGSUlzMEw2WmF0VXlNc3FTVjhCekMwUHV6clljNWRNQUpvM3VITVBhZ0EiLCJhenAiOiIxMDE5MzczNzEiLCJpc3MiOiJodHRwczovL2FjY291bnRzLmh1YXdlaS5jb20iLCJleHAiOjE1ODUyNjQwNTIsImRpc3BsYXlfbmFtZSI6IkJpcmRpZUNMIiwiaWF0IjoxNTg1MjYwNDUyLCJub25jZSI6IjYyZWNlNDA5LWZjNjAtNGE0MS05MGE4LThjZjNkMDBhM2ZiYiJ9.Djy9JD1K-IGXfjuYmMYnRUWBxfl6BkkRKQB2VrBzVsbLusA44WI3Em5YeuvHNJeZbGNxSpUSdNhRiNkZLqAS0wo9bZS-YUq5hwZ6rkun1-QuKc-m3FXQJyF0s2siJlUIGFX0WzE4NmtTy-srJgwEKPLvUwfOopr5r_9fQvMeZ4ZFV0f0zHGEhg6p4VDbK3lKu0qUD5wgVHYoTgdw2GCEZ0Kbw5XT4G2Z6VMGME_AbmyZBZldVC_MpTE3ab6BWW_FrRAnijlagQYMij4cIeGkzHuphvlpFcH-7U_AWYAd2aE0XoZH1ETcLmpyrhSBy0PIhr2ENTdCGz8GdN5KT5EjpA","openId":"MDFAMTAxOTM3MzcxQDVlMDc4MGFhYzliaZjlkZGJmZjJmNzhjYWI3YmViaY2ZjQGUxY2M4MzYyNmJiaOWU4Yzg0NTFmNWU5MGQ1ZDdhMTdkZjdmNTYzODQzMDUxNTYxNTkxOTBlYg","status":0,"unionId":"MDGtFIIs0L6ZatUyMsqSV8BzC0PuzrYc5dMAJo3uHMPagA"},"status":{"statusCode":0}}

            }
        }
    }
}

