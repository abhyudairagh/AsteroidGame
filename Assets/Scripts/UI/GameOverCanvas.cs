using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Panel that shows on gameover
    /// </summary>
    public class GameOverCanvas : UICanvas
    {
        [SerializeField]
        private TextMeshProUGUI highscoreText,scoreText;
        [SerializeField]
        private Button restartButton,exitButton;

        private void Start()
        {
            restartButton.onClick.AddListener(OnRestartGameClicked);
            exitButton.onClick.AddListener(OnExitGameClicked);
        }

        private void OnExitGameClicked()
        {
            UIManager.OnExitGame();
        }

        private void OnRestartGameClicked()
        {
            UIManager.OnRestartGame();
        }
        public void UpdateFinalScore(int score, int highScore)
        {
            highscoreText.text = highScore.ToString();
            scoreText.text = score.ToString();

        }
    }
}
