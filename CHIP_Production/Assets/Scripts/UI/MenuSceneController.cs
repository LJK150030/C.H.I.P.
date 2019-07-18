using UnityEngine;

namespace Assets.Scripts.UI
{
    public class MenuSceneController : MonoBehaviour {

        public void NextLevel(int i)
        {
            LevelSelectionController.NextLevel(i - 1);
        }

        public void ResetGame()
        {
            CheckPoint.CleanCheckPoint();
            LevelSelectionController.Reset();
        }

        public void GoToScene(string sceneName)
        {
            LevelManager.Instance.GoToLevel(sceneName);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
