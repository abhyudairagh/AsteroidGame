using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audio
{
    /// <summary>
    /// Collection of audioclips and access options
    /// </summary>
    public class AudioAssetCollection : Base.ScriptableSingleton<AudioAssetCollection>, IAudioAssetCollection
    {

        [SerializeField]
        public AudioClip[] sfxS;
        [SerializeField]
        public AudioClip[] bgmS;

        /// <summary>
        /// To get SFX clip from asset collection 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AudioClip GetSfx(int index)
        {

            try
            {
                return sfxS[index];

            }
            catch
            {          
                Debug.LogError("Could not find the SFX audio clip, Please make sure the name is correct or the clip is referenced to AudioAssetCollection");
                return null;
            }
        }

        /// <summary>
        /// To get BGM clip from asset collection 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AudioClip GetBgm(int index)
        {

            try
            {
                return bgmS[index];

            }
            catch
            {
                Debug.LogError("Could not find the BGM audio clip, Please make sure the name is correct or the clip is referenced to AudioAssetCollection");
                return null;
            }
        }

        /// <summary>
        /// Get all audioclip names in the SFX collection
        /// </summary>
        /// <returns></returns>
        public string[] GetSFXNames()
        {if (sfxS.Length > 0)
            {
                return sfxS.Select(x => x.name).ToArray();
            }
            else
            {
                return new string[] { };
            }
        }
        /// <summary>
        /// Get all audioclip names in the BGM collection
        /// </summary>
        /// <returns></returns>
        public string[] GetBGMNames()
        {
            if (bgmS.Length > 0)
            {
                return bgmS.Select(x => x.name).ToArray();
            }
            else
            {
                return new string[] { };
            }
        }
  


#if UNITY_EDITOR
        [MenuItem("Audio/Create Audio Asset Collection")]
        public static void CreateAsset()
        {
            //creates a new singleton audio collection asset in the given path
            if (Instance == null)
            {
                if (AssetDatabase.IsValidFolder("Assets/ScriptableObjects/Audio"))
                {
                    AssetDatabase.CreateAsset(new AudioAssetCollection(), "Assets/ScriptableObjects/Audio/AudioAssetCollection.asset");
                    Debug.Log("Audio asset created at path : Assets/ScriptableObjects/Audio/AudioAssetCollection.asset");
                }
                else
                {
                    Debug.LogError("<b>Audio asset creation failed</b> Directory is missing  : Assets/ScriptableObjects/Audio/");
                }
            
            }
            else
            {
                Debug.LogWarning("AudioAssetCollection object already exists at path : Assets/ScriptableObjects/Audio/AudioAssetCollection.asset");
            }
        }
#endif

    }

    public interface IAudioAssetCollection
    {
        AudioClip GetBgm(int index);
        AudioClip GetSfx(int index);
    }
}