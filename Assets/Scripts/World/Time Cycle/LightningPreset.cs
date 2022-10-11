using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Lightning Preset", menuName = "open-world/Lightning/Lightning Preset")]
public class LightningPreset: ScriptableObject {

    public Gradient ambientColor;
    public Gradient directionalColor;
    public Gradient fogColor;

}
