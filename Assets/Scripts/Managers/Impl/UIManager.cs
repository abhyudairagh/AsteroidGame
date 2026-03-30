using UI;
using UnityEngine;
using Zenject;

namespace Managers.Impl
{
    /// <summary>
    /// Controls the workflow of UI
    /// </summary>
    public class UIManager : MonoBehaviour, IUIManager
    {
        [SerializeField]
        HomeCanvas homeCanvas;

        [SerializeField]
        GameOverCanvas gameoverCanvas;

        [SerializeField]
        HUDCanvas hudCanvas;
        
        [SerializeField]
        MobileInputCanvas mobileInputCanvas;

        private UICanvas _activeCanvas;
        private IGameManager _gameManager;
        private IScoreHandler _scoreHandler;

        [Inject]
        public void Construct(IGameManager gameManager, IScoreHandler scoreHandler)
        {
            _gameManager = gameManager;
            _scoreHandler = scoreHandler;

            Initialize();
        }


        private void Initialize()
        {
            DisableAllCanvas();
            ActivateCanvas(homeCanvas);
            if (homeCanvas != null)
            {
                homeCanvas.DisplayHighScore(_scoreHandler.HighScore);
            }
            _scoreHandler.OnScoreUpdated += OnScoreUpdated;
            _scoreHandler.OnHighScoreChanged += OnHighScoreUpdated;
        }

        private void OnHighScoreUpdated()
        {
            UpdateHighScore();
        }

        private void OnScoreUpdated()
        {
            UpdateScore();
        }

        private void DisableAllCanvas()
        {
            gameoverCanvas?.SetActive(false);
            hudCanvas ?.SetActive(false);
            mobileInputCanvas?.SetActive(false);
        }

        public void OnStartGameAction()
        {
            ActivateCanvas(hudCanvas);
            _gameManager.StartGame(true);

            UpdateHUDCanvas();

#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)
        //Shows the mobile controller panel
        mobileInputCanvas.SetActive(true);
#endif
        }

        public void ShowGameOverUI()
        {
#if ((UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR)

        mobileInputCanvas.SetActive(false);
#endif
            ActivateCanvas(gameoverCanvas);
            gameoverCanvas.UpdateFinalScore(_gameManager.Score, _gameManager.HighScore);
        }

        void ActivateCanvas(UICanvas canvas)
        {
            if (canvas == null)
            {
                return;
            }
            
            _activeCanvas?.SetActive(false);

            canvas.SetActive(true);

            _activeCanvas = canvas;
        }
    
        public void OnExitGame()
        {
            ActivateCanvas(homeCanvas);
            homeCanvas.DisplayHighScore(_gameManager.HighScore);
        }
        public void OnRestartGame()
        {
            OnStartGameAction();
        }

        void UpdateHUDCanvas()
        {
            UpdateScore();
            hudCanvas.DisplayTotalLife(_gameManager.Life);
            UpdateHighScore();
        }

        private void UpdateHighScore()
        {
            hudCanvas.DisplayHighScore(_scoreHandler.HighScore);
        }

        private void UpdateScore()
        {
            hudCanvas.DisplayScore(_scoreHandler.Score);
        }

        public void OnLifeLost()
        {
            hudCanvas.OnLifeLost(_gameManager.Life);
        }
    }
}