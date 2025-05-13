using UnityEngine;
public enum PlayerEnum
{
    F35 = 0,
    AH64,
    Su57
}
[CreateAssetMenu(menuName = "ScriptableObject/Players", fileName = "Players")]
public class PlayerData : ScriptableObject
{
    public PlayerEnum player;
    [Header("Mesh Setup")]
    public GameObject mesh;

    [Header("Health Setup")]
    public int maxHealth = 100;

    [Header("Move Setup")]
    public float xMoveSpeed;
    public float jumpForce;

    [Header("Ability Setup")]
    public Ability ability;
}
