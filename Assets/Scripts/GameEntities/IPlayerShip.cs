namespace GameEntities
{
    public interface IPlayerShip : IMovable
    {
        int Health { get; }
        void ResetPlayer(bool isNewGame = false);
    }
}