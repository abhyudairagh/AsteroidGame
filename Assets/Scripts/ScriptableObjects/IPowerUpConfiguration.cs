using Managers.Impl;

namespace ScriptableObjects
{
    public interface IPowerUpConfiguration
    {
        PowerUpInfo[] PowerUpInfos { get; }
        float OccurenceDelay { get; }
    }
}