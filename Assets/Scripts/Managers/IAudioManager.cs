namespace Managers
{
    public interface IAudioManager
    {
        void PlayGameBGM();
    
        void PlaySfx(int index);

        void PlayEngineSound(bool play, bool isLoop = true);

        void StopGameBGM();

    }
}