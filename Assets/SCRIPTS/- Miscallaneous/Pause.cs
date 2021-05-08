using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Controls the Pause and Unpause state of the game while playing
public class Pause : MonoBehaviour
{
    public int MenuIndex;

    public GameObject PauseInterface;

    
    public void PauseGame()
    {
        PauseInterface.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void UnpauseGame()
    {
        PauseInterface.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ReturnToMainMenu()
    {
        PauseInterface.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(MenuIndex);
    }
}
