using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObject/Abilities/Invincible", fileName = "Ability_Invincible")]
public class Ability_Invincible : Ability
{
    //[Header("Ability의 세부 정보")]

    public override IEnumerator ActivateAbility(GameObject user) // user 인자를 받아서 사용
    {
        GameManager.isInvincible = true;
        yield return new WaitForSeconds(duration);
        GameManager.isInvincible = false;
    }
}