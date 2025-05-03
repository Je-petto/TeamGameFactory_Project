using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Obstacles", fileName = "Obstacles")]
public class ObstacleData : ScriptableObject
{
    public int damage = 10;
    public float scrollSpeed = 30f;
    public float rotationSpeed = 50f;
    public float weight = 1f;
}