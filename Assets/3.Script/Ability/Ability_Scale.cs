// Ability_ScaleData.cs 파일 -> Ability_Scale.cs 파일로 이름 변경하는 것을 추천
using UnityEngine;
using System.Collections;

// Ability 추상 클래스를 상속받습니다.
[CreateAssetMenu(menuName = "ScriptableObject/Abilities/Scale", fileName = "Ability_Scale")] // Scale 어빌리티 에셋 메뉴 추가
public class Ability_Scale : Ability
{
    [Header("Scale Ability Specific Data")]
    public float ChangeScale = 0.25f; // Scale 어빌리티에 특화된 데이터

    // Ability 클래스에서 정의한 ActivateAbility 코루틴을 구현(override)합니다.
    public override IEnumerator ActivateAbility(GameObject user) // user 인자를 받아서 사용
    {
        // 여기서 player 변수는 필요 없어집니다. user 인자를 사용합니다.
        // if (Time.time <= coolDown + lastUseTime) // CanUseAbility에서 이미 체크
        // {
            // lastUseTime = Time.time; // TryActivateAbility에서 이미 업데이트

            // user 오브젝트의 스케일을 변경합니다.
            Vector3 originalScale = user.transform.localScale; // 원래 스케일 저장
            user.transform.localScale *= ChangeScale;

            // 지속 시간만큼 기다립니다.
            yield return new WaitForSeconds(duration);

            // 원래 스케일로 되돌립니다.
            user.transform.localScale = originalScale; // 원래 스케일로 되돌림
        // }
    }

    // 필요하다면 CanUseAbility를 override하여 Scale 어빌리티만의 특정 조건을 추가할 수 있습니다.
    // public override bool CanUseAbility()
    // {
    //     // 기본 쿨타임 체크 + Scale 어빌리티만의 특정 조건
    //     return base.CanUseAbility() && !user.GetComponent<SomeComponent>().IsScaling; // 예시
    // }
}