using UnityEngine;

[CreateAssetMenu(fileName = "KeysInfoObject", menuName = "ScriptableObjects/KeysInfoObject", order = 1)]
public class KeysInfoObject : ScriptableObject {
    public int width;

    public Sprite defaultSprite;
    public KeyOverride[] keyOverrides;
    
}