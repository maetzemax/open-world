using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightingManager : MonoBehaviour {

    [SerializeField]
    private Light directionLight;

    [SerializeField]
    private LightingPreset preset;

    [SerializeField, Range(0, 24)]
    private float timeOfDay;

    private void Update() {
        if (preset == null)
            return;

        if (Application.isPlaying) {
            timeOfDay += Time.deltaTime * 0.1f;
            timeOfDay %= 24;
            UpdateLightning(timeOfDay / 24f);
        } else {
            UpdateLightning(timeOfDay / 24f);
        }
    }

    private void UpdateLightning(float timePercent) {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);
        RenderSettings.fogEndDistance = 300f;

        if (directionLight != null) {
            directionLight.color = preset.directionalColor.Evaluate(timePercent);
            directionLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }

    private void OnValidate() {
        if (directionLight != null)
            return;

        if (RenderSettings.sun != null) {
            directionLight = RenderSettings.sun;
        } else {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights) {
                directionLight = light;
                return;
            }
        }
    }

}
