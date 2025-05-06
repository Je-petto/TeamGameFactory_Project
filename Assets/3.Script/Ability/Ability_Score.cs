using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObject/Abilities/Score", fileName = "Ability_Score")]
public class Ability_Score : Ability
{
    [Header("Ability의 세부 정보")]
    public float scoreBoost = 1.5f;

    public override IEnumerator ActivateAbility(GameObject user) // user 인자를 받아서 사용
    {
        GameManager.collectableIncresePersent = scoreBoost;
        yield return new WaitForSeconds(duration);
        GameManager.collectableIncresePersent = 1f;
    }
}