using System;

namespace Managers
{
    public interface IScoreHandler
    {
        event Action OnHighScoreChanged;
        event Action OnScoreUpdated;
        int Score { get; }
        int HighScore { get; set; }
        void AddPoints(int points);
        void ResetScore();
    }
}