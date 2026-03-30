using System.Collections;
using System.Collections.Generic;
using GameEntities;
using GameEntities.Impl;
using Helpers;
using ScriptableObjects;
using UnityEngine;
using Utility.Pooling;
using Zenject;

namespace Managers.Impl
{
    /// <summary>
    /// Manage the asteroids spawning in the scene
    /// </summary>
    public class AsteroidController : MonoBehaviour, IAsteroidController
    {
        [SerializeField]
        private List<Asteroid> prefabSet;
        [SerializeField]
        private ParticleSystem particleSystemPrefab;
    
        private IPlayerManager _playerManager;
        private IGameManager _gameManager;

        private WaitForSeconds _wait;
        private int _totalActiveAsteroidsCount;

        private bool _isSpawningAsteroids;
        private ISpawningPositionUtility _spawningPositionUtility;
        private IAudioManager _audioManager;
        private IAsteroidObjectPoolProvider _asteroidObjectPoolProvider;
        private IAsteroidConfiguration _asteroidConfiguration;

        [Inject]
        public void Construct(
            IPlayerManager playerManager,
            IGameManager gameManager,
            ISpawningPositionUtility spawningPositionUtility,
            IAudioManager audioManager,
            IAsteroidObjectPoolProvider asteroidObjectPoolProvider,
            IAsteroidConfiguration asteroidConfiguration)
        {
            _playerManager = playerManager;
            _gameManager = gameManager;
            _spawningPositionUtility = spawningPositionUtility;
            _audioManager = audioManager;
            _asteroidObjectPoolProvider = asteroidObjectPoolProvider;
            _asteroidConfiguration = asteroidConfiguration;
            _asteroidObjectPoolProvider.MaxCount = 100;
        }
    
        private void Start()
        {
            _gameManager.OnGameReset += OnGameReset;
            _asteroidObjectPoolProvider.OnAsteroidDestroyed += OnAsteroidDestroyed;
        }
   

        private void OnGameReset()
        {
            _isSpawningAsteroids = false;
            StopAllCoroutines();
            _asteroidObjectPoolProvider.Reset();
        }
    
        public void Initialize()
        {
            _wait = new WaitForSeconds(_asteroidConfiguration.AsteroidSpawningInterval);
        }
    
        /// <summary>
        /// Used to create/spawn Asteroids 
        /// </summary>
        public void SpawnEnemies()
        {
            _totalActiveAsteroidsCount = _asteroidConfiguration.TotalLargeAsteroids;
            if (_totalActiveAsteroidsCount > 0)
            {
                StartCoroutine(SetupLargeAsteroids());
            }
        }

        private void CreateAsteroid(AsteroidType type, Vector3 position = default)
        {
            //Create asteroids for the first time and use it as object pooling
            //Add the large asteroids to collection and add the sub asteroids(small and medium) as reference to its parent asteroid
            var spawnedAsteroid =  _asteroidObjectPoolProvider.Get(type).Value;
            if (spawnedAsteroid == null)
            {
                return;
            }
            
            if (type == AsteroidType.Large)
            {
                PlaceLargeAsteroid(spawnedAsteroid);
            }
            else
            {
                PlaceSubParticles(spawnedAsteroid, position);
            }
        }

        private void PlaceLargeAsteroid(IAsteroid spawnedAsteroid)
        {
            Vector3 pos = _spawningPositionUtility.GetAsteroidRandomSpawningPosition();
            Vector2 dir = (_playerManager.PlayerShip.Transform.position - pos).normalized;
            spawnedAsteroid.Initialize(pos, dir);
        }
    
        void PlaceSubParticles(IAsteroid subParticle, Vector3 position)
        {
            Vector3 pos = position;
            Vector2 dir = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            subParticle.Initialize(pos, dir);
        }

        IEnumerator SetupLargeAsteroids()
        {
            //Logic to spawn asteroids in an interval of time

            if (_isSpawningAsteroids)
                yield break;


            _isSpawningAsteroids = true;
            var spawnCount = 0;
            while (_gameManager.IsGameStarted && spawnCount < _totalActiveAsteroidsCount)
            {
                yield return SpawnAsteroids(AsteroidType.Large);
                spawnCount++;
            }
            _isSpawningAsteroids = false;
        }

        IEnumerator SpawnAsteroids(AsteroidType type)
        {
            CreateAsteroid(type);
            yield return _wait;
        }

        private void OnAsteroidDestroyed(IAsteroid asteroid)
        {
            if(!_gameManager.IsGameStarted)
                return;

            var asteroidType = asteroid.TypeConfig.asteroidType;
            _gameManager.UpdateScore(asteroid.TypeConfig.scoreValue);

            ShowParticleFX(asteroid.Transform.position);
            _audioManager.PlaySfx(asteroid.TypeConfig.sfx);

            if (asteroidType == AsteroidType.Small)
            {
                if (IsLargeAsteroidReadyToSpawn() && _asteroidObjectPoolProvider.TotalActiveAsteroids(AsteroidType.Large) < _asteroidConfiguration.MaxAllowedLargeAsteroids)
                {
                    StartCoroutine(SpawnAsteroids(AsteroidType.Large));
                }

                if (_asteroidObjectPoolProvider.TotalActiveAsteroids(AsteroidType.Large) <=
                    _asteroidConfiguration.TotalLargeAsteroids)
                {
                    //create new large asteroids to create difficulty
                    StartCoroutine(SpawnAsteroids(AsteroidType.Large));

                }
            }
            else
            {
                var type = asteroidType == AsteroidType.Large ? AsteroidType.Medium :
                    AsteroidType.Small;
                var count = asteroidType == AsteroidType.Large ? _asteroidConfiguration.MediumSubParticles : _asteroidConfiguration.SmallSubParticles;
                for (int i = 0; i < count; i++)
                {
                    CreateAsteroid(type, asteroid.Transform.position);
                } 
            }
        }

  


        private void ShowParticleFX(Vector3 pos)
        {
            if (particleSystemPrefab != null)
            {
                Instantiate(particleSystemPrefab, pos, Quaternion.identity);
            }
        }

   
        /// <summary>
        /// Returns true if large asteroids are ready to spawn
        /// </summary>
        /// <returns></returns>
        private bool IsLargeAsteroidReadyToSpawn()
        {
            var neededMediumAsteroids = _asteroidConfiguration.MediumSubParticles;
            var neededSmallAsteroids = _asteroidConfiguration.SmallSubParticles * neededMediumAsteroids;
        
            var availableMediumAsteroids = _asteroidObjectPoolProvider.TotalInActiveAsteroids(AsteroidType.Medium);
            var availableSmallAsteroids = _asteroidObjectPoolProvider.TotalInActiveAsteroids(AsteroidType.Small);

            if (availableMediumAsteroids >= neededMediumAsteroids && availableSmallAsteroids >= neededSmallAsteroids)
            {
                return true;
            }

            return false;
        }
    
        private void OnDestroy()
        {
            _gameManager.OnGameReset -= OnGameReset;
            _asteroidObjectPoolProvider.OnAsteroidDestroyed -= OnAsteroidDestroyed;

        }

    }
}