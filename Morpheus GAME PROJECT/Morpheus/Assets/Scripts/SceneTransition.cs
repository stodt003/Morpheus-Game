using UnityEngine;
using System.Collections;

public class SceneTransition : MonoBehaviour {

    public void TransitionToScene(string sceneName)
    {
        Debug.Log("Transitioning to Scene:  " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
    public void ExitApplication()
    {
        Debug.Log("Application Quitting");
        Application.Quit();
    }
}
