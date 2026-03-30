using System;

namespace Managers
{
    public interface IGameManager
    {
        event Action OnGameReset;
    
        bool IsGameStarted { get; }
        int  HighScore { get; }
        int Life { get; }
        int Score { get; }
        void UpdateScore(int points);
        void StartGame(bool isNewGame = false);
        void LostLife();
        void GameOver();

    }
}