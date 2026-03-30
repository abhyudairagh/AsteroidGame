using System;
using UnityEngine;

namespace Managers.Impl
{
    /// <summary>
    /// Holds the score inside the game
    /// </summary>
    public class ScoreHandler : IScoreHandler
    {
        public event Action OnHighScoreChanged;
        public event Action OnScoreUpdated;

        private const string HighScoreKey = "***HIGH SCORE***";
        private int _score;
        public int Score => _score;
        public int HighScore
        {
            get
            {
                if (PlayerPrefs.HasKey(HighScoreKey))
                {
                    return PlayerPrefs.GetInt(HighScoreKey);
                }
                return 0;       
            }

            set => PlayerPrefs.SetInt(HighScoreKey,value);
        }

        private void ValidateHighScore()
        {
            if(_score > HighScore)
            {
                HighScore = _score;
                OnHighScoreChanged?.Invoke();
            }
        }

        public void AddPoints(int point)
        {
            _score += point;
            ValidateHighScore();
            OnScoreUpdated?.Invoke();
        }
        public void ResetScore()
        {
            _score = 0;
            OnScoreUpdated?.Invoke();
        }
    }
}