using Android.App;
using Android.Content.PM;
using Android.OS;
namespace BeThere;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using BeThere.Models;
using Plugin.LocalNotification;
using Android.Provider;
using BeThere.Platforms.Android;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        // Other code...

        // Request notification permission (Android 8.0 and higher)
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
            if (!notificationManager.AreNotificationsEnabled())
            {
                Intent intent = new Intent(Settings.ActionAppNotificationSettings);
                StartActivity(intent);
            }
        }
    }

}

