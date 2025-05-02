using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Obstacles", fileName = "Obstacles")]
public class ObstacleData : ScriptableObject
{
    public int damage = 10;
    public float rotationSpeed = 50f;
}
