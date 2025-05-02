using UnityEngine;

public enum CollectableType {SCORE, HEALTH, }

[CreateAssetMenu(menuName = "ScriptableObject/Collectable", fileName = "Collectable")]
public class CollectableData : ScriptableObject
{
    public CollectableType type;
    public float rotationSpeed = 60f;
    public float weight = 1f;
}
