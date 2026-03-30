namespace ScriptableObjects
{
    public interface IAsteroidConfiguration
    {
        float AsteroidSpawningInterval { get; }
        int MaxAllowedLargeAsteroids { get; }
        int TotalLargeAsteroids { get; }
        int MediumSubParticles { get; }
        int SmallSubParticles { get; }
    }
}