using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using Microsoft.AspNetCore.SignalR.Client;


namespace BeThere.Platforms.Android
{
    [Service]
    public class MyForegroundService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // Implement foreground service logic here.
            // You should start the service as a foreground service with a notification.

            // Example: Start the service as foreground
            Notification notification = new Notification();
            StartForeground(1, notification);

            return StartCommandResult.Sticky;
        }
    }
}
