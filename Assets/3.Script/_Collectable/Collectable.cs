using UnityEngine;

public abstract class Collectable : MonoBehaviour //추상 클래스 abstract 
{
    public CollectableData data; //Collectable(item)의 속성 담음
    private Vector3 randomRotationAxis; // 회전
    public virtual void Awake()
    {
        randomRotationAxis = Random.insideUnitSphere.normalized; //무작위 회전축 설정
        if (randomRotationAxis == Vector3.zero)
            randomRotationAxis = Vector3.up; //회전이 0이되면 안되므로 y축으로 설정
    } // virtual로 선언되어 상속받은 클래스에서 오버라이드 하여 사용
    public virtual void Update() 
    {
        transform.Rotate(randomRotationAxis, data.rotationSpeed * Time.deltaTime);
    } // virtual로 선언되어 상속받은 클래스에서 오버라이드 하여 사용
    public virtual void ClearObstacles() { } // 장에물 제거용 동작 구현용 메소드. 상속받은 클래스에서 세팅
    public virtual void Reverse(PlayerBehaviour player) { } // 아이템 획득시에 좌우반전
}