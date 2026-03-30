namespace ScriptableObjects
{
    public interface IPlayerConfiguration
    {
        int TotalLife { get; }
        float MaxSpeed { get; }
        int DeathSfx { get; }
        int GameOverSfx { get; }
        int MoveSensitivity { get; }
        int SteerSensitivity { get; }
        int BreakingSensitivity { get; }
        int FiringRate { get; }
        bool BurstFire { get; }
        int BurstFireCapacity { get; }
        float BulletSpeed { get; }
        float BulletLifespan { get; }
        int FireSfx { get; }
    }
}