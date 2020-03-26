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
using Com.Huawei.Hms.Support.Hwid.Result;
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
                var authHuaweiIdTask = HuaweiIdAuthManager.ParseAuthResultFromIntent(data);
                if (authHuaweiIdTask.IsSuccessful)
                {
                    //The sign-in is successful, and the user's HUAWEI ID information and ID token are obtained.
                    AuthHuaweiId huaweiAccount = authHuaweiIdTask.GetResult<AuthHuaweiId>();
                    Log.Info(TAG, "idToken:" + huaweiAccount.IdToken);
                }
                else
                {
                    //The sign-in failed.      
                    Log.Error(TAG, "sign in failed : " + ((ApiException)authHuaweiIdTask.Exception).StatusCode);
                }
            }
        }
    }
}

