using System.Collections;
using Audio;
using UnityEngine;
using Zenject;

namespace Managers.Impl
{
    /// <summary>
    /// Controls the audio playing functionality
    /// </summary>
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [SerializeField]       
        private float initialBeatDelay;

        [SerializeField]
        private float minimumBeatDelay;

        [SerializeField]
        private float rateOfTempoChange;
        [SerializeField]
        private AudioSource bgmSource;
        [SerializeField]
        private AudioSource sfxSource;
        [SerializeField]
        private AudioSource engineSource;
    
        [SerializeField]
        [Audio(AudioType.BGM)]
        private int bgmBeat1;
        [SerializeField]
        [Audio(AudioType.BGM)]
        private int bgmBeat2;
        [SerializeField]
        [Audio(AudioType.SFX)]
        private int engineSound;

        private float _beatDelay;

        private IAudioAssetCollection _audioAssetCollection;

        [Inject]
        public void Construct(IAudioAssetCollection audioAssetCollection)
        {
            _audioAssetCollection  = audioAssetCollection;
        }
    
        public void PlayEngineSound(bool play, bool isLoop = true)
        {
            if(engineSource == null)
                return;
        
            if (play)
            {
                engineSource.clip = _audioAssetCollection.GetSfx(engineSound);
                engineSource.loop = isLoop;
                engineSource.Play();
            }
            else
            {
                engineSource.Stop();
            }
        }
    
        public void PlaySfx(int index)
        {
            sfxSource.PlayOneShot(_audioAssetCollection.GetSfx(index));
        }

        private void PlayBGM(int index, bool isLoop = true)
        {
            bgmSource.clip = _audioAssetCollection.GetBgm(index);
            bgmSource.loop = isLoop;

            bgmSource.Play();
        }

        public void PlayGameBGM()
        {
            //Logic to mix 2 beats to make a bgm in the game

            _beatDelay = initialBeatDelay;

            float duration1 = _audioAssetCollection.GetBgm(bgmBeat1).length;
            float duration2 = _audioAssetCollection.GetBgm(bgmBeat2).length;

            StartCoroutine(PlayBeats(duration1, duration2));

        }

        private IEnumerator PlayBeats(float duration1, float duration2)
        {
            while (true)
            {
                PlayBGM(bgmBeat2, false);

                yield return new WaitForSeconds(duration1);
                yield return new WaitForSeconds(_beatDelay);

                PlayBGM(bgmBeat1, false);
                yield return new WaitForSeconds(duration2);
                yield return new WaitForSeconds(_beatDelay);


                _beatDelay -= rateOfTempoChange;
                _beatDelay = Mathf.Clamp(_beatDelay, minimumBeatDelay, initialBeatDelay);
            }
        }

        public void StopGameBGM()
        {
            bgmSource.Stop();
            StopAllCoroutines();
        }

    }
}