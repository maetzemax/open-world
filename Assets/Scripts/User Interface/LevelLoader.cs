using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class LevelLoader : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider slider;

    public void LoadLevel(int sceneIndex) {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex) {

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone) {

            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            yield return null;
        }
    }

    public void RestAllData() {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        File.Delete(Application.dataPath + "/Inventory_Data.xml");
        File.Delete(Application.dataPath + "/World_Data.xml");

    }
}
