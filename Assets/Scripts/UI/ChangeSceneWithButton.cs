using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeSceneWithButton: MonoBehaviour
{
    [SerializeField] private Button loadButton;

    private void Awake() {
        if (loadButton != null) {
            loadButton.interactable = PlayerPrefs.HasKey("seed");
        }
    }

    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void CloseGame() {
        Application.Quit();
    }
}
