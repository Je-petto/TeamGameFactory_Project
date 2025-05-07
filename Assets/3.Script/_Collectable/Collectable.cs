using UnityEngine;

public abstract class Collectable : MonoBehaviour //추상 클래스 abstract 
{
    public CollectableData data; //Collectable(item)의 속성 담음
    public virtual void Awake() {} // virtual로 선언되어 상속받은 클래스에서 오버라이드 하여 사용
    public virtual void Update() {} // virtual로 선언되어 상속받은 클래스에서 오버라이드 하여 사용
    public virtual void ClearObstacles() { } // 장에물 제거용 동작 구현용 메소드. 상속받은 클래스에서 세팅
    public virtual void Reverse(PlayerBehaviour player) { } // 아이템 획득시에 좌우반전
}