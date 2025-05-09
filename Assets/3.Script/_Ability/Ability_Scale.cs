// Ability_ScaleData.cs 파일 -> Ability_Scale.cs 파일로 이름 변경하는 것을 추천
using UnityEngine;
using System.Collections;

// Ability 추상 클래스를 상속받습니다.
[CreateAssetMenu(menuName = "ScriptableObject/Abilities/Scale", fileName = "Ability_Scale")] // Scale 어빌리티 에셋 메뉴 추가
public class Ability_Scale : Ability
{
    [Header("Ability's Detailed Setup")]
    public float ChangeScale = 0.25f; // Scale 어빌리티에 특화된 데이터

    public override IEnumerator ActivateAbility(GameObject user) // user 인자를 받아서 사용
    {
        // user 오브젝트의 스케일을 변경합니다.
        Vector3 originalScale = user.transform.localScale; // 원래 스케일 저장
        user.transform.localScale *= ChangeScale;

        // 지속 시간만큼 기다립니다.
        yield return new WaitForSeconds(duration);

        // 원래 스케일로 되돌립니다.
        user.transform.localScale = originalScale; // 원래 스케일로 되돌림
    }
}