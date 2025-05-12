using UnityEngine;
using System.Collections;

// Ability 추상 클래스를 상속받습니다.
[CreateAssetMenu(menuName = "ScriptableObject/Abilities/Scale", fileName = "Ability_Scale")] // Scale 어빌리티 에셋 메뉴 추가
public class Ability_Scale : Ability
{
    [Header("Ability's Detailed Setup")]
    public float changeScale = 0.25f; // Scale 어빌리티에 특화된 데이터
    public float increseXSpeed = 1.25f;

    public override IEnumerator ActivateAbility(GameObject user) // user 인자를 받아서 사용
    {
        PlayerBehaviour player = user.GetComponent<PlayerBehaviour>();
        float originalSpeed = player.data[GameManager.Instance.selectPlayer].xMoveSpeed;
        
        // Data 변경
        player.data[GameManager.Instance.selectPlayer].xMoveSpeed = increseXSpeed;
        Vector3 originalScale = user.transform.localScale;
        user.transform.localScale *= changeScale;

        // 지속 시간만큼 기다립니다.
        yield return new WaitForSeconds(duration);

        // Data 초기화
        player.data[GameManager.Instance.selectPlayer].xMoveSpeed = originalSpeed;
        user.transform.localScale = originalScale;
    }
}