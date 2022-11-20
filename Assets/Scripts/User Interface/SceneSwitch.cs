using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {
    public void ChangeToSceneWithID(int index) {
        SceneManager.LoadScene(index);
    }

    public void CloseApplication() {
        Application.Quit();
    }
}