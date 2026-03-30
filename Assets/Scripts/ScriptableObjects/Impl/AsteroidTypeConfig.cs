using UnityEngine;
using Random = UnityEngine.Random;
public enum AsteroidType
{
    Small = 0,
    Medium,
    Large
}

/// <summary>
/// Holds the config data for android
/// </summary>
[CreateAssetMenu(fileName = "NewAsteroidConfig", menuName = "GameConfiguration/AsteroidConfig")]
public class AsteroidTypeConfig : ScriptableObject
{
    public int scoreValue;
    public float MinSpeed;
    public float MaxSpeed;
    public Sprite[] textures;
    public AsteroidType asteroidType;
    [Audio(AudioType.SFX)]
    public int sfx;

    public Sprite GetTexture()
    {
        return textures[Random.Range(0, textures.Length)];
    }

}
