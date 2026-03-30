using GameEntities;

namespace Factory
{
    public interface IAsteroidFactory
    {
        IAsteroid Create(AsteroidType asteroidType);
    }
}