
namespace Managers
{
    public interface IUIManager
    {
        void OnStartGameAction();

        void OnExitGame();

        void OnLifeLost();

        void OnRestartGame();

        void ShowGameOverUI();

    }
}