// Ability.cs 파일 (새로 생성하거나 주석 해제 후 수정)
using UnityEngine;
using System.Collections; // 코루틴을 사용하려면 이것도 필요
using CustomInspector;

public abstract class Ability : ScriptableObject // ScriptableObject를 상속받아 에셋으로 관리 가능하게 함
{
    [Header("Ability의 기본 정보")]
    public float duration = 3f; // 기본 지속 시간
    public float coolDown = 3f; // 기본 쿨타임

    public abstract IEnumerator ActivateAbility(GameObject user); // 어떤 오브젝트가 어빌리티를 쓰는지 인자로 넘겨줄 수 있음
}

public enum Abilities
{
    Scale = 0,
    Invincible
}