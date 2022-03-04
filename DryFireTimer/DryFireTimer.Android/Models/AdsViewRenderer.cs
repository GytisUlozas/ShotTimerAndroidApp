using Android.Content;
using Android.Gms.Ads;
using DryFireTimer.Droid.Models;
using DryFireTimer.Models;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdBanner), typeof(AdsViewRenderer))]
namespace DryFireTimer.Droid.Models
{
    public class AdsViewRenderer : ViewRenderer
    {
        
        public AdsViewRenderer(Context _context) : base(_context)
        {
            
        }
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            var adView = new AdView(Context)
            {
                AdUnitId = "ca-app-pub-3940256099942544/6300978111",
                AdSize = AdSize.Banner
        };
            var requestbuilder = new AdRequest.Builder();
            adView.LoadAd(requestbuilder.Build());
            SetNativeControl(adView);
        }
    }
}