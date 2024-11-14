using UnityEngine;

public class SceneController : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {
        AppData.currentSpeed = 1;
        Time.timeScale = AppData.currentSpeed;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
}
