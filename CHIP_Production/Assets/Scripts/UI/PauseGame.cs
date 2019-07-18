using UnityEngine;

namespace Assets.Project_Assets_Folder.Scripts
{
    public class PauseGame : MonoBehaviour {
        public void PasueGame()
        {
            Time.timeScale = 0.0f;
        }

        public void UnpauseGame()
        {
            Time.timeScale = 1.0f;
        }
    }
}
