using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WorldParser : MonoBehaviour {

    public static WorldParser instance;

    [SerializeField] public int seed;
    [SerializeField] public Transform player;

    public InputField seedInput;

    private void Awake() {
        instance = this;

        if(PlayerPrefs.HasKey("seed")) {
            seed = PlayerPrefs.GetInt("seed");
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadWorld() {
        UpdateSeed();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("seed", seed);
        PlayerPrefs.Save();
        LoadScene("World");
    }

    void UpdateSeed() {
        seed = int.Parse(seedInput.text);
    }

    void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
