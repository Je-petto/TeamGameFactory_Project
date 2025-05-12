using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "ScriptableObject/Abilities/Score", fileName = "Ability_Score")]
public class Ability_Score : Ability
{
    [Header("Ability's Detailed Setup")]
    public float scoreBoost = 1.5f;

    public override IEnumerator ActivateAbility(GameObject user) // user 인자를 받아서 사용
    {
        GameManager.Instance.collectableIncresePersent = scoreBoost;
        yield return new WaitForSeconds(duration);
        GameManager.Instance.collectableIncresePersent = 1f;
    }
}