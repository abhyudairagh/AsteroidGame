using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Managers.Impl
{
    /// <summary>
    /// Handles game mechanism and workflow
    /// </summary>
    public class GameManager : MonoBehaviour, IGameManager
    {
        private const float RespawnDelay = 2f;

        public event Action OnGameReset;
    
        private bool _isGameStarted;
        public bool IsGameStarted => _isGameStarted;

        private IScoreHandler _scoreHandler;

        public int HighScore => _scoreHandler.HighScore;

        public int Score => _scoreHandler.Score;

        public int Life
        {
            get
            {
                if(_playerManager == null || _playerManager.PlayerShip == null)
                {
                    return 0;
                }
                return _playerManager.PlayerShip.Health;
            }
        }

        // Start is called before the first frame update
        private IAsteroidController _asteroidController;
        private IPlayerManager _playerManager;
        private IUIManager _uiManager;
        private IAudioManager _audioManager;
        private IPowerUpController _powerUpManager;
        private IPlayerConfiguration _playerConfig;

        [Inject]
        public void Construct(
            IAsteroidController asteroidController,
            IPlayerManager playerManager,
            IUIManager uiManager,
            IAudioManager audioManager,
            IPowerUpController powerUpController,
            IScoreHandler scoreHandler,
            IPlayerConfiguration playerConfig
        )
        {
            _asteroidController = asteroidController;
            _playerManager = playerManager;
            _uiManager = uiManager;
            _audioManager = audioManager;
            _powerUpManager = powerUpController;
            _scoreHandler = scoreHandler;
            _playerConfig = playerConfig;
        }

        /// <summary>
        /// Funtion to start/reset the game
        /// </summary>
        /// <param name="isNewGame"></param>
        public void StartGame(bool isNewGame = false)
        {
            _isGameStarted = true;

            if (isNewGame)
            {
                _scoreHandler.ResetScore();
                _audioManager.PlayGameBGM();
            }

            _playerManager.StartPlayer(isNewGame);
            _asteroidController.SpawnEnemies();
            _powerUpManager.StartSpawningPowerUps();

        
        }

        /// <summary>
        ///Call when player lost a life 
        /// </summary>
        public void LostLife()
        {
            _audioManager.PlaySfx(_playerConfig.DeathSfx);

            _isGameStarted = false;
            OnGameReset?.Invoke(); 
            _uiManager.OnLifeLost();

            StartCoroutine(ResetGame());
        }

        IEnumerator ResetGame()
        {     
            yield return new WaitForSeconds(RespawnDelay);
            StartGame();
        }

        /// <summary>
        /// Call when the game meets the condition of gameover
        /// </summary>
        public void GameOver()
        {
            _audioManager.PlaySfx(_playerConfig.GameOverSfx);

            _isGameStarted = false;
            OnGameReset?.Invoke();
            _uiManager.ShowGameOverUI();
            _audioManager.StopGameBGM();

        }

        /// <summary>
        /// Update the score in the game
        /// </summary>
        /// <param name="points"></param>
        public void UpdateScore(int points)
        {
            if (_isGameStarted)
            {
                _scoreHandler.AddPoints(points);
            }  
        
        }
    }
}