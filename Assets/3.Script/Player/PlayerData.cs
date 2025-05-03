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
    [Header("Health Setup")]
    public int maxHealth = 100;

    [Header("Move Setup")]
    public float xMoveSpeed;
    public float jumpForce;



}
