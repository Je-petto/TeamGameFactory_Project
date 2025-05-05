// Ability.cs 파일 (새로 생성하거나 주석 해제 후 수정)
using UnityEngine;
using System.Collections; // 코루틴을 사용하려면 이것도 필요

public abstract class Ability : ScriptableObject // ScriptableObject를 상속받아 에셋으로 관리 가능하게 함
{
    [Header("Base Ability Data")]
    public float duration = 3f; // 기본 지속 시간
    public float coolDown = 3f; // 기본 쿨타임
    protected float lastUseTime; // 마지막 사용 시간 초기화

    // 어빌리티 발동 메소드를 추상 메소드로 정의
    // 상속받는 클래스는 반드시 이 메소드를 구현해야 합니다.
    // public abstract void UseAbility(); // 만약 일반 메소드로 한다면

    // 코루틴으로 어빌리티 발동 메소드를 정의할 수도 있습니다.
    // 이 경우 상속받는 클래스에서 override해서 코루틴 로직을 구현합니다.
    public virtual void Awake()
    {
        lastUseTime = 0f;
    }
    public abstract IEnumerator ActivateAbility(GameObject user); // 어떤 오브젝트가 어빌리티를 쓰는지 인자로 넘겨줄 수 있음

    // 쿨타임 체크 메소드 (선택 사항)
    public virtual bool CanUseAbility()
    {
        Debug.Log($"{Time.time} >= {lastUseTime} + {coolDown}");
        return Time.time >= lastUseTime + coolDown;
    }

    // 어빌리티 사용을 시도하는 메소드 (코루틴 실행 포함)
    // PlayerBehaviour에서 이 메소드를 호출하게 합니다.
    public Coroutine ActivateAbility(MonoBehaviour userMonoBehaviour)
    {
        if (CanUseAbility())
        {
            // lastUseTime = Time.time;
            Debug.Log($"{GetType().Name} 어빌리티 사용!");
            // 코루틴 실행 및 반환
            return userMonoBehaviour.StartCoroutine(ActivateAbility(userMonoBehaviour.gameObject));
        }
        return null; // 사용 실패 시 null 반환
    }

    // 필요하다면 비활성화, 업데이트 메소드 등을 추가할 수 있습니다.
    // public virtual void Deactivate() {}
    // public virtual void UpdateAbility() {}
}

public enum Abilities
{
    Scale = 0,
    Invincible
}