// Ability.cs 파일 (새로 생성하거나 주석 해제 후 수정)
using UnityEngine;
using System.Collections; // 코루틴을 사용하려면 이것도 필요

public abstract class Ability : ScriptableObject // ScriptableObject를 상속받아 에셋으로 관리 가능하게 함
{
    [Header("Ability's Based Setup")]
    public float duration = 3f; // 기본 지속 시간
    public float coolDown = 3f; // 기본 쿨타임

    // PlayerBeHaviour에서 능력을 사용할 때, 반드시 실행되는 메서드
    // 따라서, 모든 Ability는 해당 메서드를 가지고 있어야 된다.
    // 이 메서드는 Ability의 주 작동을 담당함.
    public abstract IEnumerator ActivateAbility(GameObject user); // 어떤 오브젝트가 어빌리티를 쓰는지 인자로 넘겨줄 수 있음
}