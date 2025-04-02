using Android.App;
using Android.Content.PM;
using AndroidContent = Android.Content;
namespace DerpRaven.Maui.Platforms.Android;

[Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)]
[IntentFilter(new[] { AndroidContent.Intent.ActionView },
    Categories = new[] {
            AndroidContent.Intent.CategoryDefault,
            AndroidContent.Intent.CategoryBrowsable
        },
    DataScheme = CALLBACK_SCHEME)]
public class WebAuthenticationCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
{
    const string CALLBACK_SCHEME = "myapp";
}
