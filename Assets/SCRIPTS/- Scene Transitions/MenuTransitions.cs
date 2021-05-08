using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuTransitions : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1f; // Real time
    }

    // Function to transition to a level
    public int SceneIndex; 
    public void TransitionTo()
    {
        SceneManager.LoadScene(SceneIndex);
    }
    // Function to Exit the Application
    public void ExitGame()
    {
        Application.Quit(0);
    }

    // TRANSITION TO OPTIONS SCENE
    public void Options_Interface()
    {
        SceneManager.LoadScene("Options");
    }

    // TRASITION TO TROPHIES SCENE
    public void Trophies_Interface()
    {

    }

    // TRANSITION TO STORE SCENE
    public void Store_Interface()
    {

    }

    // TRANSITION TO POWERUPS SCENE
    public void PowerUps_Interface()
    {

    }
}
