using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneRestarter : MonoBehaviour
{
    public int sceneIndex;
  
    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.R))
      {
            SceneManager.LoadScene(sceneIndex);
      }
    }
}
