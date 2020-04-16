using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Huawei.Agconnect.Config;
using Com.Huawei.Hms.Ads;

namespace Birdie.Huawei.HMSSDK.Demo
{
    [Application(Name = "cl.birdie.huawei.hmssdk.demo.MainApplication")]
    public class MainApplication : Application
    {
        protected MainApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            var config = AGConnectServicesConfig.FromContext(this);
            config.OverlayWith(new MyLazyInputStream(this));
        }

        //protected override void AttachBaseContext(Context @base)
        //{
        //    //base.AttachBaseContext(@base);
        //}

        //protected override void AttachBaseContext(Context context)
        //{
        //    base.AttachBaseContext(context);
        //    var config = AGConnectServicesConfig.FromContext(context);
        //    config.OverlayWith(new MyLazyInputStream(context));
        //}
    }

    public class MyLazyInputStream : LazyInputStream
    {
        public MyLazyInputStream(Context ctx) : base(ctx)
        {

        }

        public override Stream Get(Context context)
        {
            try
            {
                return context.Assets.Open("agconnect-services.json");
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine($"excepcion: {e}");
                return null;
            }
        }
    }
}