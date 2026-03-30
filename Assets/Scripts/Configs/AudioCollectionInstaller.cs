using Audio;
using UnityEngine;
using Zenject;

namespace Configs
{
    public class AudioCollectionInstaller : ScriptableObjectInstaller<AudioCollectionInstaller>
    {
        [SerializeField]
        private AudioAssetCollection audioCollection;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<AudioAssetCollection>().FromInstance(audioCollection).AsSingle();
        }
    }
}