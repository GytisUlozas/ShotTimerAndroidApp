namespace DryFireTimer.Models
{
    public interface IMyAudioPlayer
    {
        void Load(string fileName);
        void Play();
        void Unload();
    }
}
