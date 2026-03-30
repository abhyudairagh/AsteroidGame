using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Canvas that shows at Home scene
    /// </summary>
    public class HomeCanvas : UICanvas
    {
        [SerializeField]
        private TextMeshProUGUI highscoreText;
        [SerializeField]
        private Button playButton;

        private void Start()
        {
            playButton.onClick.AddListener(OnGameStartClicked);
        }

        private void OnGameStartClicked()
        {
            UIManager.OnStartGameAction();
        }

        public void DisplayHighScore(int score)
        {
            highscoreText.text = score.ToString(); ;
        }
  
    }
}
