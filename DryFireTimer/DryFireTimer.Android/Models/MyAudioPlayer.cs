using Android.Content.Res;
using Android.Media;
using DryFireTimer.Droid.Models;
using DryFireTimer.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(MyAudioPlayer))]
namespace DryFireTimer.Droid.Models
{
    public class MyAudioPlayer : IMyAudioPlayer
    {
        private MediaPlayer player;
        private AssetFileDescriptor descriptor;

        public void Load(string fileName)
        {
            player = new MediaPlayer();
            descriptor = Android.App.Application.Context.Assets.OpenFd(fileName);
            player.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
            player.Prepare();
        }
        public void Play()
            => player.Start();
        
        public void Unload()
        {
            player.Release();
            player.Dispose();
        }
    }
}