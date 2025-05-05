using UnityEngine;

public enum CollectableType {SCORE, HEALTH, DOBS, REVERS}

[CreateAssetMenu(menuName = "ScriptableObject/Collectable", fileName = "Collectable")]
public class CollectableData : ScriptableObject
{
    public CollectableType type;
    public float scrollSpeed = 30f;
    public float rotationSpeed = 60f;
    public float weight = 1f;
}
