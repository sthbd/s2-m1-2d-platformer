using UnityEngine;
using UnityEngine.SceneManagement;


public class ResetGame : MonoBehaviour
{
    public KeyCode ResetKey = KeyCode.R;
    void Update()
    {
        // if r key is pressed, it will reload the current scene
        if (Input.GetKeyDown(ResetKey) == true)
        {
            // get current scene 
            Scene currentScene = SceneManager.GetActiveScene();
            // Reset the  scene
            SceneManager.LoadScene(currentScene.buildIndex);
            

         }
    }
}
