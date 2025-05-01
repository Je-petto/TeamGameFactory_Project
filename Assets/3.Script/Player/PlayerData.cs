using Unity.VisualScripting;
using UnityEngine;
public enum PlayerEnum
{
    Airship = 0
}
[CreateAssetMenu(menuName = "ScriptableObject/PlayerData", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public PlayerEnum player = PlayerEnum.Airship;
    [Header("Health Setup")]
    public int maxHealth = 100;

    [Header("Move Setup")]
    public float xMoveSpeed;
    public float jumpForce;

    

}
